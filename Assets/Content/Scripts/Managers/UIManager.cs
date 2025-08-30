using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{

    [Header("TextUI")]
    [SerializeField] TMP_Text distanceText;
    [SerializeField] TMP_Text cointText;

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

    public void UpdateDistanceText(float distance)
    {
        if (Time.time >= nextUpdateTime)
        {
            distanceText.text = Mathf.FloorToInt(distance).ToString();
            nextUpdateTime = Time.time + updateRate;
        }
    }

    public void UpdateCoinText(int coinCount) => cointText.text = coinCount.ToString();



    public void ShowUI() => gameObject.SetActive(true);

    public void HideUI() => gameObject.SetActive(false);

    public void ToggleUI() => gameObject.SetActive(!gameObject.activeSelf);

    public bool IsUIActive() => gameObject.activeSelf;
}
