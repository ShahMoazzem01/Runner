// CoinPool.cs  (serializable pool class; same file or separate, as you prefer)
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CoinPool
{
    public GameObject coinPrefab;
    public int poolSize = 50;
    public Transform coinParent;

    private Queue<Coin> availableCoins = new Queue<Coin>();
    private List<Coin> allCoins = new List<Coin>();

    public void Initialize()
    {
        if (coinPrefab == null)
        {
            Debug.LogError("Coin prefab is not assigned in CoinPool!");
            return;
        }

        if (coinParent == null)
        {
            GameObject parentObj = new GameObject("CoinPool");
            coinParent = parentObj.transform;
        }

        for (int i = 0; i < poolSize; i++)
            CreateNewCoin();

        Debug.Log("Coin pool initialized with " + allCoins.Count + " coins.");
    }

    private void CreateNewCoin()
    {
        GameObject coinObj = GameObject.Instantiate(coinPrefab, coinParent);
        Coin coin = coinObj.GetComponent<Coin>();

        if (coin == null)
        {
            Debug.LogError("Coin prefab doesn't have a Coin component!");
            GameObject.Destroy(coinObj);
            return;
        }

        coin.pool = this;
        coinObj.SetActive(false);
        availableCoins.Enqueue(coin);
        allCoins.Add(coin);
    }

    public Coin GetCoin()
    {
        if (availableCoins.Count == 0)
        {
            CreateNewCoin();
            Debug.LogWarning("Coin pool expanded. New size: " + allCoins.Count);
        }

        Coin coin = availableCoins.Dequeue();
        if (coin == null)
            return GetCoin(); // defensive

        // Activate and return
        coin.gameObject.SetActive(true);
        // Optionally detach from pool parent so it's easier to inspect in hierarchy when active:
        coin.transform.SetParent(null);
        return coin;
    }

    public void ReturnToPool(Coin coin)
    {
        if (coin == null || coin.gameObject == null) return;

        // If already inactive, don't enqueue again (prevents double-enqueue)
        if (!coin.gameObject.activeInHierarchy)
            return;

        coin.gameObject.SetActive(false);
        coin.transform.SetParent(coinParent); // keep in pool folder
        availableCoins.Enqueue(coin);
    }

    public int GetActiveCoinCount()
    {
        return allCoins.Count - availableCoins.Count;
    }
}
