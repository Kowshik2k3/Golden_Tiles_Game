
/*
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    [Header("Music Playlist")]
    public AudioClip[] musicClips;
    public AudioClip selectedClip;

    public static AudioClip SelectedMusic => Instance != null ? Instance.selectedClip : null;

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
            return; // important!
        }
    }

    private void OnEnable()
    {
        // Rebind buttons every time a new scene loads
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Reset music selection when returning home
        if (scene.name == "HomeScene")
            selectedClip = null;

        // ✅ Rebind all buttons in HomeScene automatically
        var buttons = FindObjectsOfType<UnityEngine.UI.Button>();
        foreach (var btn in buttons)
        {
            var buttonIndex = btn.GetComponent<MusicButtonIndex>();
            if (buttonIndex != null)
            {
                btn.onClick.RemoveAllListeners();
                int index = buttonIndex.index; // capture
                btn.onClick.AddListener(() => SelectMusic(index));
            }
        }
    }

    public void SelectMusic(int index)
    {
        if (index >= 0 && index < musicClips.Length)
        {
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
*/



using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    [Header("Music Playlist")]
    public AudioClip[] musicClips;
    public AudioClip selectedClip;

    public static AudioClip SelectedMusic => Instance != null ? Instance.selectedClip : null;

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
            return; // important!
        }
    }

    private void OnEnable()
    {
        // Rebind buttons every time a new scene loads
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Reset music selection when returning home
        if (scene.name == "HomeScene")
            selectedClip = null;

        // ✅ Rebind all buttons in HomeScene automatically
        var buttons = FindObjectsOfType<UnityEngine.UI.Button>();
        foreach (var btn in buttons)
        {
            var buttonIndex = btn.GetComponent<MusicButtonIndex>();
            if (buttonIndex != null)
            {
                btn.onClick.RemoveAllListeners();
                int index = buttonIndex.index; // capture
                btn.onClick.AddListener(() => SelectMusic(index));
            }
        }
    }

    public void SelectMusic(int index)
    {
        if (index >= 0 && index < musicClips.Length)
        {
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
