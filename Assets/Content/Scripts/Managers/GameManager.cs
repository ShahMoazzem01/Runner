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

    public GameState CurrentState { get; private set; } = GameState.Playing;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        // DontDestroyOnLoad(gameObject);
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

    public void SetPause(bool isPaused)
    {
        if (isPaused)
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
        Time.timeScale = 0f; // Freeze game
        UIManager.Instance.ShowGameOverScreen();
    }
}




