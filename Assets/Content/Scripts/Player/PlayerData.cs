using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public float totalDistance = 0f;
    public int totalCoins = 0;
    public float bestDistance = 0f;
    public int bestCoins = 0;

    public void AddRunData(float runDistance, int coins)
    {
        totalDistance += runDistance;
        totalCoins += coins;

        if (runDistance > bestDistance)
            bestDistance = runDistance;

        if (coins > bestCoins)
            bestCoins = coins;

        Save(); // save after every update
    }

    public void Save()
    {
        PlayerPrefs.SetFloat("TotalDistance", totalDistance);
        PlayerPrefs.SetInt("TotalCoins", totalCoins);
        PlayerPrefs.SetFloat("BestDistance", bestDistance);
        PlayerPrefs.SetInt("BestCoins", bestCoins);
        PlayerPrefs.Save();
    }

    public void Load()
    {
        totalDistance = PlayerPrefs.GetFloat("TotalDistance", 0f);
        totalCoins = PlayerPrefs.GetInt("TotalCoins", 0);
        bestDistance = PlayerPrefs.GetFloat("BestDistance", 0f);
        bestCoins = PlayerPrefs.GetInt("BestCoins", 0);
    }

    public void ResetAllData()
    {
        totalDistance = 0f;
        totalCoins = 0;
        bestDistance = 0f;
        bestCoins = 0;
        Save();
    }
}
