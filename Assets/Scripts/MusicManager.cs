using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    [Header("Background Music")]
    public AudioSource backgroundSource;

    [Header("Demo Preview")]
    public AudioSource demoSource;
    public float demoDuration = 10f;
    public float demoFadeOutTime = 2f;

    [Header("Music Playlist")]
    public AudioClip[] musicClips;
    public AudioClip selectedClip;

    public static AudioClip SelectedMusic => Instance != null ? Instance.selectedClip : null;

    private int currentBgIndex = 0;
    private Coroutine demoRoutine;
    private SongCardUI currentDemoCard;
    private bool demoPlaying = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        demoSource.loop = false;
        backgroundSource.loop = false;
    }

    private void Start()
    {
        StartBackgroundPlaylist();
    }

    private void Update()
    {
        if (!demoPlaying && !backgroundSource.isPlaying && musicClips.Length > 0)
        {
            PlayNextBackgroundTrack();
        }
    }

    // ---------------- BACKGROUND PLAYLIST ----------------

    void StartBackgroundPlaylist()
    {
        currentBgIndex = 0;
        backgroundSource.clip = musicClips[currentBgIndex];
        backgroundSource.Play();
    }

    void PlayNextBackgroundTrack()
    {
        currentBgIndex++;

        if (currentBgIndex >= musicClips.Length)
            currentBgIndex = 0;

        backgroundSource.clip = musicClips[currentBgIndex];
        backgroundSource.Play();
    }

    // ---------------- DEMO PREVIEW ----------------

    public void PlayDemo(AudioClip clip, SongCardUI card)
    {
        StopDemo();

        demoPlaying = true;
        currentDemoCard = card;

        backgroundSource.Pause();

        demoSource.clip = clip;
        demoSource.volume = 1f;
        demoSource.Play();

        demoRoutine = StartCoroutine(DemoLoopRoutine());
    }

    IEnumerator DemoLoopRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(demoDuration - demoFadeOutTime);

            float t = 0f;
            float startVol = demoSource.volume;

            while (t < demoFadeOutTime)
            {
                t += Time.deltaTime;
                demoSource.volume = Mathf.Lerp(startVol, 0f, t / demoFadeOutTime);
                yield return null;
            }

            demoSource.Stop();
            demoSource.volume = 1f;
            demoSource.Play();
        }
    }
    public void StopBackgroundMusic()
    {
        if (backgroundSource != null)
        {
            backgroundSource.Stop();
            backgroundSource.clip = null;
        }
    }


    public void StopDemo()
    {
        if (demoRoutine != null)
        {
            StopCoroutine(demoRoutine);
            demoRoutine = null;
        }

        if (demoSource.isPlaying)
            demoSource.Stop();

        demoSource.volume = 1f;
        demoPlaying = false;

        if (currentDemoCard != null)
        {
            currentDemoCard.ForceStopDemo();
            currentDemoCard = null;
        }

        backgroundSource.UnPause();
    }

    // ---------------- GAME MUSIC SELECTION ----------------

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "HomeScene")
            selectedClip = null;
    }
    public void SelectMusic(int index)
    {
        if (index >= 0 && index < musicClips.Length)
        {
            StopDemo();
            StopBackgroundMusic();   // ⬅ ABSOLUTELY REQUIRED

            selectedClip = musicClips[index];

            SceneManager.LoadScene("SampleScene");
        }
        else
        {
            Debug.LogWarning("Invalid music index selected!");
        }
    }



    public void ResetMusicSelection()
    {
        selectedClip = null;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
