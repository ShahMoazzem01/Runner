using UnityEngine;

public enum GameState { Playing, GameOver, Paused }

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game Settings")]
    [Header("1 = normal speed, 4 = 4x Speed")]
    [Range(1f, 4f)]
    public float gameSpeed = 1f;
    public float baseSpeed = 5f;

    [Header("Player Run Tracking")]
    public float distanceRun = 0f;
    public int coinCount = 0;

    [Header("Player Data (Totals)")]
    public PlayerData playerData = new PlayerData();

    public GameState CurrentState { get; private set; } = GameState.Playing;

    [Header("Game Over")]
    [SerializeField] public GameObject pauseButton;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        // DontDestroyOnLoad(gameObject);

        playerData.Load(); // load saved values at startup
    }

    void Start()
    {
        pauseButton.SetActive(true);
    }

    void OnEnable()
    {
        GameEvents.OnDeadlyHit += SetGameOver;
    }

    void OnDisable()
    {
        GameEvents.OnDeadlyHit -= SetGameOver;
    }



    void Update()
    {
        switch (CurrentState)
        {
            case GameState.Playing:
                //trac distance run every fram
                distanceRun += baseSpeed * gameSpeed * Time.deltaTime;
                UIManager.Instance.UpdateDistanceText(distanceRun);
                break;
            case GameState.Paused:
                break;
            case GameState.GameOver:
                break;
        }

        //trac distance run every fram
        // distanceRun += baseSpeed * gameSpeed * Time.deltaTime;
        // UIManager.Instance.UpdateDistanceText(distanceRun);
    }

    public void SetSpeed(float speed)
    {
        gameSpeed = Mathf.Clamp(speed, 1f, 4f);
    }

    public void onCoinCollected()
    {
        coinCount += 1;
        UIManager.Instance.UpdateCoinText(coinCount);
    }

    public void ToggelPuse()
    {
        if (GameState.Paused != CurrentState)
        {
            CurrentState = GameState.Paused;
            Time.timeScale = 0f; // Stops physics, animations, Update calls that use deltaTime
            UIManager.Instance.ShowPauseMenu();
        }
        else
        {
            CurrentState = GameState.Playing;
            Time.timeScale = 1f; // Resumes physics, animations, Update calls that use deltaTime
            UIManager.Instance.HidePauseMenu();
        }

    }

    public void SetGameOver()
    {
        CurrentState = GameState.GameOver;
        pauseButton.SetActive(false);
        Time.timeScale = 0f;

        // Save run data (updates totals + bests inside PlayerData)
        playerData.AddRunData(distanceRun, coinCount);

        // Pass everything to UI
        UIManager.Instance.ShowGameOverScreen(
            distanceRun, coinCount,
            playerData.bestDistance, playerData.bestCoins,
            playerData.totalDistance, playerData.totalCoins
        );
    }


    public void ResetRunData()
    {
        distanceRun = 0f;
        coinCount = 0;
    }

    public void ResetAllPlayerData()
    {
        playerData.ResetAllData();
        ResetRunData();
    }
}




