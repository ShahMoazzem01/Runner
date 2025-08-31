using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("UIPanels")]
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] GameObject hudPanel;
    [SerializeField] GameObject pausePanel;

    [Header("TextUI")]
    [SerializeField] TMP_Text distanceText;
    [SerializeField] TMP_Text cointText;


    [Header("GameOverUI")]
    [SerializeField] TMP_Text distanceRecordText;   // this run’s distance
    [SerializeField] TMP_Text coinRecordText;       // this run’s coins
    [SerializeField] TMP_Text bestCoinText;         // best single-run coins
    [SerializeField] TMP_Text bestDistanceText;     // best single-run distance
    [SerializeField] TMP_Text totalDistanceText;    // lifetime distance
    [SerializeField] TMP_Text totalCoinText;        // lifetime coins

    [Header("Settings")]
    [SerializeField] float updateRate = 0.1f;
    float nextUpdateTime = 0;

    public static UIManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        // DontDestroyOnLoad(gameObject);
    }

    void Start()
    {

        gameOverPanel.SetActive(false);
        pausePanel.SetActive(false);
    }

    void OnEnable()
    {

    }

    void OnDisable()
    {

    }

    public void UpdateDistanceText(float distance)
    {
        if (Time.time >= nextUpdateTime)
        {
            distanceText.text = "Distance: " + Mathf.FloorToInt(distance).ToString();
            nextUpdateTime = Time.time + updateRate;
        }
    }

    public void UpdateCoinText(int coinCount) => cointText.text = "Coins: " + coinCount.ToString();


    public void ShowGameOverScreen(float runDistance, int runCoins,
        float bestDistance, int bestCoins,
        float totalDistance, int totalCoins)
    {
        Debug.Log($"Run={runDistance}, Coins={runCoins}, Best={bestDistance}/{bestCoins}, Total={totalDistance}/{totalCoins}");

        gameOverPanel.SetActive(true);
        hudPanel.SetActive(false);

        // This run
        distanceRecordText.text = "Run Distance: " + runDistance.ToString("F0");
        coinRecordText.text = "Run Coins: " + runCoins;

        // Best run stats
        bestDistanceText.text = "Best Distance: " + bestDistance.ToString("F0");
        bestCoinText.text = "Best Coins: " + bestCoins;

        // Lifetime totals
        totalDistanceText.text = "Total Distance: " + totalDistance.ToString("F0");
        totalCoinText.text = "Total Coins: " + totalCoins;

    }

    public void HideGameOverScreen()
    {
        gameOverPanel.SetActive(false);
        hudPanel.SetActive(true);
    }
    public void ShowPauseMenu() => pausePanel.SetActive(true);
    public void HidePauseMenu() => pausePanel.SetActive(false);



    public void ShowUI() => gameObject.SetActive(true);

    public void HideUI() => gameObject.SetActive(false);

    public void ToggleUI() => gameObject.SetActive(!gameObject.activeSelf);

    public bool IsUIActive() => gameObject.activeSelf;
}
