using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("UI Text References")]
    [SerializeField] TMPro.TextMeshProUGUI bestDistanceText, bestCoinText, totalDistanceText, totalCoinText;

    [Header("Stats")]
    float bestDistance, bestCoins, totalDistance, totalCoins;

    [Header("Panels")]
    [SerializeField] GameObject playerInfoPanel;
    [SerializeField] GameObject aboutPanel;

    void Start()
    {
        playerInfoPanel.SetActive(false);
        aboutPanel.SetActive(false);
    }

    // === Player Info Panel ===
    public void ShowPlayerInfo()
    {
        playerInfoPanel.SetActive(true);
        Load();

        // Best run stats
        bestDistanceText.text = "Best Distance: " + bestDistance.ToString("F0");
        bestCoinText.text = "Best Coins: " + bestCoins;

        // Lifetime totals
        totalDistanceText.text = "Total Distance: " + totalDistance.ToString("F0");
        totalCoinText.text = "Total Coins: " + totalCoins;
    }

    public void HidePlayerInfo()
    {
        playerInfoPanel.SetActive(false);
    }

    public void TogglePlayerInfo()
    {
        if (playerInfoPanel.activeSelf)
            HidePlayerInfo();
        else
            ShowPlayerInfo();
    }

    // === About Panel ===
    public void ShowAbout()
    {
        aboutPanel.SetActive(true);
    }

    public void HideAbout()
    {
        aboutPanel.SetActive(false);
    }

    public void ToggleAbout()
    {
        aboutPanel.SetActive(!aboutPanel.activeSelf);
    }

    // === Load Player Stats ===
    public void Load()
    {
        totalDistance = PlayerPrefs.GetFloat("TotalDistance", 0f);
        totalCoins = PlayerPrefs.GetInt("TotalCoins", 0);
        bestDistance = PlayerPrefs.GetFloat("BestDistance", 0f);
        bestCoins = PlayerPrefs.GetInt("BestCoins", 0);
    }
}
