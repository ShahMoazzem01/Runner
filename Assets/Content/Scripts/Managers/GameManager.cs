using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game Settings")]

    [Header("1 = normal speed, 4 = 4x Speed")]
    [Range(1f, 4f)]
    public float gameSpeed = 1f;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetSpeed(float speed)
    {
        gameSpeed = Mathf.Clamp(speed, 1f, 4f);
    }
}
