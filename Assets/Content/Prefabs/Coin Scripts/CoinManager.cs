// CoinManager.cs
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public CoinPool coinPool;

    [Header("Generation Settings")]
    public float playAreaWidth = 12f;
    public float coinGap = 4f;
    public float generationDistance = 50f;
    public float coinHeight = 1f;
    public float worldMoveSpeed;

    [Header("Generation Probabilities")]
    [Range(0, 100)] public int oneLaneProbability = 40;
    [Range(0, 100)] public int twoLaneProbability = 20;
    [Range(0, 100)] public int threeLaneProbability = 10;

    [Header("Coin Count Range")]
    public int minCoins = 5;
    public int maxCoins = 15;

    [Header("Debug")]
    public bool enableDebug = true;

    private float laneWidth;
    private float lastSpawnZ; // frontier where last coin was spawned (world coord)
    private List<Coin> activeCoins = new List<Coin>();
    private int skipProbability;

    void Start()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager.Instance is null!");
            enabled = false;
            return;
        }

        worldMoveSpeed = GameManager.Instance.baseSpeed * GameManager.Instance.gameSpeed;
        laneWidth = playAreaWidth / 3f;
        skipProbability = 100 - (oneLaneProbability + twoLaneProbability + threeLaneProbability);

        if (coinPool != null)
        {
            coinPool.Initialize();
            // DebugLog("Coin pool initialized with size: " + coinPool.poolSize);
        }
        else
        {
            Debug.LogError("CoinPool reference is not set in CoinManager!");
        }

        // Start the frontier at player's Z and fill ahead until generationDistance:
        lastSpawnZ = player.position.z;
        while (lastSpawnZ < player.position.z + generationDistance)
        {
            GenerateCoinSet();
        }
    }

    void Update()
    {
        if (GameManager.Instance == null) return;
        worldMoveSpeed = GameManager.Instance.baseSpeed * GameManager.Instance.gameSpeed;
        if (coinPool == null || player == null) return;

        // Move frontier backward with the world movement so spawning continues as coins move -Z
        lastSpawnZ -= worldMoveSpeed * Time.deltaTime;

        // Ensure coins are at least generationDistance ahead of player
        while (lastSpawnZ < player.position.z + generationDistance)
        {
            GenerateCoinSet();
        }

        // DebugLog($"lastSpawnZ: {lastSpawnZ:F2}  threshold: {player.position.z + generationDistance:F2}  activeCoins: {activeCoins.Count}");

        RemoveOldCoins();
        MoveCoins();
    }

    void GenerateCoinSet()
    {
        int pattern = ChoosePattern();
        int coinsInSet = Random.Range(minCoins, maxCoins + 1);

        // DebugLog("Generating coin set. Pattern: " + pattern + ", Coins in set: " + coinsInSet);

        if (pattern == 0) // Skip area
        {
            // Advance frontier for skipped distance
            lastSpawnZ += coinsInSet * coinGap;
            // DebugLog("Skipping coin generation. New lastSpawnZ: " + lastSpawnZ);
            return;
        }

        List<int> lanesToUse = GetLanesForPattern(pattern);

        for (int i = 0; i < coinsInSet; i++)
        {
            // For each coin step forward along Z by coinGap, then spawn coins for each lane
            lastSpawnZ += coinGap;
            float zPos = lastSpawnZ;

            foreach (int lane in lanesToUse)
            {
                float xPos = (lane - 1) * laneWidth; // -playAreaWidth/3, 0, +playAreaWidth/3

                Coin coin = coinPool.GetCoin();
                if (coin != null)
                {
                    // Remove any stale occurrences of this coin in activeCoins (prevents duplicate movement)
                    for (int j = activeCoins.Count - 1; j >= 0; j--)
                    {
                        if (activeCoins[j] == coin)
                            activeCoins.RemoveAt(j);
                    }

                    coin.transform.position = new Vector3(xPos, coinHeight, zPos);
                    coin.laneIndex = lane;
                    activeCoins.Add(coin);
                    // DebugLog("Coin generated at position: " + coin.transform.position);
                }
                else
                {
                    Debug.LogError("CoinPool.GetCoin() returned null!");
                }
            }
        }

        // DebugLog("Finished generating coins. LastSpawnZ: " + lastSpawnZ);
    }

    void RemoveOldCoins()
    {
        for (int i = activeCoins.Count - 1; i >= 0; i--)
        {
            Coin coin = activeCoins[i];
            if (coin == null)
            {
                activeCoins.RemoveAt(i);
                continue;
            }

            // If coin went behind player by 10 units or got deactivated, return it
            if (!coin.gameObject.activeInHierarchy || coin.transform.position.z < player.position.z - 10f)
            {
                coinPool.ReturnToPool(coin);
                activeCoins.RemoveAt(i);
            }
        }
    }

    void MoveCoins()
    {
        // Move coins toward negative Z (world moves backward)
        float moveSpeed = worldMoveSpeed;
        for (int i = activeCoins.Count - 1; i >= 0; i--)
        {
            Coin coin = activeCoins[i];
            if (coin != null && coin.gameObject.activeInHierarchy)
            {
                coin.transform.Translate(0, 0, -moveSpeed * Time.deltaTime, Space.World);
            }
        }
    }

    List<int> GetLanesForPattern(int pattern)
    {
        List<int> lanes = new List<int>();

        switch (pattern)
        {
            case 1: lanes.Add(Random.Range(0, 3)); break;
            case 2:
                int first = Random.Range(0, 3);
                lanes.Add(first);
                int second;
                do { second = Random.Range(0, 3); } while (second == first);
                lanes.Add(second);
                break;
            case 3:
                lanes.Add(0); lanes.Add(1); lanes.Add(2);
                break;
        }

        return lanes;
    }

    int ChoosePattern()
    {
        int randomValue = Random.Range(0, 100);
        if (randomValue < oneLaneProbability) return 1;
        if (randomValue < oneLaneProbability + twoLaneProbability) return 2;
        if (randomValue < oneLaneProbability + twoLaneProbability + threeLaneProbability) return 3;
        return 0; // Skip
    }

    void DebugLog(string message)
    {
        if (enableDebug) Debug.Log(message);
    }
}
