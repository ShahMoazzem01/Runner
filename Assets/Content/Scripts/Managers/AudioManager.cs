using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    // Singleton Instance
    public static AudioManager Instance { get; private set; }

    [Header("Audio source")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;

    [Header("Audio clips")]
    [SerializeField] AudioClip[] musicClips;
    [SerializeField] AudioClip coinSound;
    [SerializeField] AudioClip jumpSound;
    [SerializeField] AudioClip clickSound;
    [SerializeField] AudioClip hitSound;

    private void Awake()
    {
        // Ensure only one instance exists
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Keep audio manager across scenes
    }

    private void Start()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;

        if (sceneIndex == 0 && musicClips.Length > 0)
        {
            PlayMusic(musicClips[0]);
        }
        else if (musicClips.Length > 1)
        {
            int randomIndex = Random.Range(1, musicClips.Length);
            PlayMusic(musicClips[randomIndex]);
        }
    }

    public void PlayMusic(AudioClip clip)
    {
        if (clip == null) return;

        musicSource.clip = clip;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;

        sfxSource.PlayOneShot(clip);
    }

    public void PlayCoinSound() => PlaySFX(coinSound);
    public void PlayJumpSound() => PlaySFX(jumpSound);
    public void PlayClickSound() => PlaySFX(clickSound);
    public void PlayHitSound() => PlaySFX(hitSound);
}
