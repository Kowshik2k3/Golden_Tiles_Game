/*

using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public enum GameState { Menu, Playing, GameOver, LevelTransition, Win }
    public GameState CurrentState = GameState.Menu;

    [Header("Gameplay")]
    public float baseTileFallSpeed = 6f;
    [HideInInspector] public float tileFallSpeed;
    public TileSpawner tileSpawner;

    [Header("Audio")]
    public AudioSource musicSource;
    public AudioClip[] musicClips; // 🆕 store multiple music clips
    private int selectedSongIndex = 0; // 🆕 selected song

    [Header("UI")]
    public GameObject menuPanel;
    public GameObject startButton;
    public TMP_Text levelText;
    public TMP_Text scoreText;
    public GameObject levelTransitionPanel;
    public TMP_Text transitionText;
    public GameObject winPanel;
    public GameObject gameOverPanel;
    public TMP_Text finalScoreText;

    private int currentLevelIndex = 0;
    private LevelData[] levels;
    private bool levelCompleteTriggered = false;
    private int score = 0;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        // 🆕 Load selected song index from PlayerPrefs (set in Home Scene)
        selectedSongIndex = PlayerPrefs.GetInt("SelectedSong", 0);

        // 🆕 Assign selected music clip safely
        if (musicClips != null && musicClips.Length > 0 && selectedSongIndex < musicClips.Length)
        {
            musicSource.clip = musicClips[selectedSongIndex];
        }

        // configure levels
        levels = new LevelData[]
        {
            new LevelData("Easy", 1.1f, 1.1f),
            new LevelData("Medium", 1.3f, 1.3f),
            new LevelData("Hard", 1.5f, 1.7f)
        };

        // initial UI state
        if (winPanel != null) winPanel.SetActive(false);
        if (levelTransitionPanel != null) levelTransitionPanel.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);

        if (scoreText != null) scoreText.text = "0";
        if (finalScoreText != null) finalScoreText.text = "Score: 0";

        if (menuPanel != null) menuPanel.SetActive(true);
        if (startButton != null) startButton.SetActive(true);

        CurrentState = GameState.Menu;
    }

    public void StartGame()
    {
        // 🆕 Load the selected song chosen in Home Scene
        if (MusicManager.SelectedMusic != null)
        {
            musicSource.clip = MusicManager.SelectedMusic;
        }
        else
        {
            // fallback in case nothing is selected
            selectedSongIndex = PlayerPrefs.GetInt("SelectedSong", 0);
            if (musicClips != null && musicClips.Length > 0 && selectedSongIndex < musicClips.Length)
            {
                musicSource.clip = musicClips[selectedSongIndex];
            }
        }

        // Hide UI panels
        if (menuPanel != null) menuPanel.SetActive(false);
        if (startButton != null) startButton.SetActive(false);

        // Reset score
        score = 0;
        if (scoreText != null) scoreText.text = "0";
        if (finalScoreText != null) finalScoreText.text = "Score: 0";

        currentLevelIndex = 0;
        CurrentState = GameState.Playing;

        // Start the first level
        StartCoroutine(StartLevel(currentLevelIndex));
    }

    private IEnumerator StartLevel(int index)
    {
        if (index >= levels.Length)
        {
            HandleWin();
            yield break;
        }

        LevelData level = levels[index];
        if (levelText != null) levelText.text = "Level: " + level.LevelName;

        CurrentState = GameState.Playing;
        levelCompleteTriggered = false;

        // speed setup
        if (musicSource != null) musicSource.pitch = level.MusicSpeed;
        tileFallSpeed = baseTileFallSpeed * level.TileSpeed;

        // 🆕 use selected song clip safely
        if (musicSource != null && musicSource.clip != null)
        {
            musicSource.Play();
        }

        if (tileSpawner != null) tileSpawner.StartSpawning();

        // 🆕 CALCULATE DURATION WITH 2 SECOND EARLY STOP
        float musicDuration = (musicSource != null && musicSource.clip != null)
            ? (musicSource.clip.length / Mathf.Max(0.0001f, musicSource.pitch))
            : 0f;

        float spawnDuration = musicDuration - 2f; // Stop spawning 2 seconds early
        spawnDuration = Mathf.Max(1f, spawnDuration); // Ensure at least 1 second

        float timer = 0f;

        // 🆕 SPAWNING PHASE
        while (timer < spawnDuration && !levelCompleteTriggered && CurrentState == GameState.Playing)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        // 🆕 STOP SPAWNING 2 SECONDS BEFORE MUSIC ENDS
        if (tileSpawner != null) tileSpawner.StopSpawning();

        // 🆕 WAIT FOR REMAINING TIME (2 seconds for existing tiles to clear)
        float remainingTime = musicDuration - spawnDuration;
        while (timer < musicDuration && !levelCompleteTriggered && CurrentState == GameState.Playing)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        if (CurrentState == GameState.Playing)
        {
            if (musicSource != null) musicSource.Stop();
            yield return StartCoroutine(LevelTransition());
        }
    }
    public void LevelComplete()
    {
        if (CurrentState != GameState.Playing) return;
        levelCompleteTriggered = true;
    }

    public void LoadHomeScene()
    {
        if (MusicManager.Instance != null)
        {
            MusicManager.Instance.ResetMusicSelection();
        }
        SceneManager.LoadScene("HomeScene"); // your menu scene name

    }

    private IEnumerator LevelTransition()
    {
        CurrentState = GameState.LevelTransition;

        if (currentLevelIndex >= levels.Length - 1)
        {
            HandleWin();
            yield break;
        }

        if (levelTransitionPanel != null)
        {
            levelTransitionPanel.SetActive(true);
            if (transitionText != null)
                transitionText.text = "Well done! Get ready for the next challenging level...";
        }

        yield return new WaitForSeconds(3f);

        if (levelTransitionPanel != null) levelTransitionPanel.SetActive(false);

        currentLevelIndex++;
        StartCoroutine(StartLevel(currentLevelIndex));
    }

    private void HandleWin()
    {
        CurrentState = GameState.Win;
        if (winPanel != null) winPanel.SetActive(true);
        if (levelText != null) levelText.text = "Level: Completed";
    }

    public void ReplayGame()
    {
        if (winPanel != null) winPanel.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);

        score = 0;
        if (scoreText != null) scoreText.text = "0";
        if (finalScoreText != null) finalScoreText.text = "Score: 0";

        currentLevelIndex = 0;
        StartCoroutine(StartLevel(currentLevelIndex));
    }

    public void RestartScene()
    {
        if (tileSpawner != null) tileSpawner.StopSpawning();
        if (musicSource != null) musicSource.Stop();

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void AddScore(int amount)
    {
        if (CurrentState != GameState.Playing) return;

        score += amount;
        if (scoreText != null) scoreText.text = score.ToString();
    }

    public void GameOver()
    {
        if (CurrentState != GameState.Playing) return;

        CurrentState = GameState.GameOver;

        if (tileSpawner != null) tileSpawner.StopSpawning();
        if (musicSource != null) musicSource.Stop();

        if (finalScoreText != null) finalScoreText.text = "Score: " + score.ToString();

        if (gameOverPanel != null) gameOverPanel.SetActive(true);

        Debug.Log("Game Over triggered (score: " + score + ")");
    }
}

[System.Serializable]
public class LevelData
{
    public string LevelName;
    public float MusicSpeed;
    public float TileSpeed;

    public LevelData(string name, float musicSpeed, float tileSpeed)
    {
        LevelName = name;
        MusicSpeed = musicSpeed;
        TileSpeed = tileSpeed;
    }
}
*/
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public enum GameState { Menu, Playing, GameOver, LevelTransition, Win, GameOverSequence }
    public GameState CurrentState = GameState.Menu;

    [Header("Gameplay")]
    public float baseTileFallSpeed = 6f;
    [HideInInspector] public float tileFallSpeed;
    public TileSpawner tileSpawner;

    [Header("Audio")]
    public AudioSource musicSource;
    public AudioSource sfxSource;
    public AudioClip gameOverBeep;
    public AudioClip[] musicClips;
    private int selectedSongIndex = 0;

    [Header("UI")]
    public GameObject menuPanel;
    public GameObject startButton;
    public TMP_Text levelText;
    public TMP_Text scoreText;
    public GameObject levelTransitionPanel;
    public TMP_Text transitionText;
    public GameObject winPanel;
    public GameObject gameOverPanel;
    public TMP_Text finalScoreText;

    private int currentLevelIndex = 0;
    private LevelData[] levels;
    private bool levelCompleteTriggered = false;
    private int score = 0;
    private TileController missedTile; // Reference to the tile that caused game over

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        // Ensure we have SFX audio source
        if (sfxSource == null)
        {
            sfxSource = gameObject.AddComponent<AudioSource>();
        }

        selectedSongIndex = PlayerPrefs.GetInt("SelectedSong", 0);

        if (musicClips != null && musicClips.Length > 0 && selectedSongIndex < musicClips.Length)
        {
            musicSource.clip = musicClips[selectedSongIndex];
        }

        levels = new LevelData[]
        {
            new LevelData("Easy", 1.1f, 1.1f),
            new LevelData("Medium", 1.3f, 1.3f),
            new LevelData("Hard", 1.5f, 1.7f)
        };

        if (winPanel != null) winPanel.SetActive(false);
        if (levelTransitionPanel != null) levelTransitionPanel.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);

        if (scoreText != null) scoreText.text = "0";
        if (finalScoreText != null) finalScoreText.text = "Score: 0";

        if (menuPanel != null) menuPanel.SetActive(true);
        if (startButton != null) startButton.SetActive(true);

        CurrentState = GameState.Menu;
    }

    public void StartGame()
    {
        if (MusicManager.SelectedMusic != null)
        {
            musicSource.clip = MusicManager.SelectedMusic;
        }
        else
        {
            selectedSongIndex = PlayerPrefs.GetInt("SelectedSong", 0);
            if (musicClips != null && musicClips.Length > 0 && selectedSongIndex < musicClips.Length)
            {
                musicSource.clip = musicClips[selectedSongIndex];
            }
        }

        if (menuPanel != null) menuPanel.SetActive(false);
        if (startButton != null) startButton.SetActive(false);

        score = 0;
        if (scoreText != null) scoreText.text = "0";
        if (finalScoreText != null) finalScoreText.text = "Score: 0";

        currentLevelIndex = 0;
        CurrentState = GameState.Playing;

        // NEW: Reset progress bar and stars
        if (ProgressStarManager.Instance != null)
        {
            ProgressStarManager.Instance.ResetProgress();
        }

        // NEW: Reset greetings tap count
        ResetGreetings();

        // Start the first level
        StartCoroutine(StartLevel(currentLevelIndex));
    }

    private IEnumerator StartLevel(int index)
    {
        if (index >= levels.Length)
        {
            HandleWin();
            yield break;
        }

        LevelData level = levels[index];
        if (levelText != null) levelText.text = "Level: " + level.LevelName;

        CurrentState = GameState.Playing;
        levelCompleteTriggered = false;

        // NEW: Update progress bar for new level
        if (ProgressStarManager.Instance != null)
        {
            ProgressStarManager.Instance.UpdateProgress(0f, index);
        }

        // speed setup
        if (musicSource != null) musicSource.pitch = level.MusicSpeed;
        tileFallSpeed = baseTileFallSpeed * level.TileSpeed;

        if (musicSource != null && musicSource.clip != null)
        {
            musicSource.Play();
        }

        if (tileSpawner != null) tileSpawner.StartSpawning();

        float musicDuration = (musicSource != null && musicSource.clip != null)
            ? (musicSource.clip.length / Mathf.Max(0.0001f, musicSource.pitch))
            : 0f;

        float spawnDuration = musicDuration - 2f;
        spawnDuration = Mathf.Max(1f, spawnDuration);

        float timer = 0f;

        // SPAWNING PHASE
        while (timer < spawnDuration && !levelCompleteTriggered && CurrentState == GameState.Playing)
        {
            timer += Time.deltaTime;

            // NEW: Update progress during level
            if (ProgressStarManager.Instance != null)
            {
                float levelProgress = timer / spawnDuration;
                ProgressStarManager.Instance.UpdateProgress(levelProgress, index);
            }

            yield return null;
        }

        if (tileSpawner != null) tileSpawner.StopSpawning();

        float remainingTime = musicDuration - spawnDuration;
        while (timer < musicDuration && !levelCompleteTriggered && CurrentState == GameState.Playing)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        if (CurrentState == GameState.Playing)
        {
            if (musicSource != null) musicSource.Stop();

            // NEW: Award star for completing this level
            if (ProgressStarManager.Instance != null)
            {
                ProgressStarManager.Instance.AwardStar(index);
            }

            yield return StartCoroutine(LevelTransition());
        }
    }

    public void LevelComplete()
    {
        if (CurrentState != GameState.Playing) return;
        levelCompleteTriggered = true;
    }

    public void LoadHomeScene()
    {
        if (MusicManager.Instance != null)
        {
            MusicManager.Instance.ResetMusicSelection();
        }
        SceneManager.LoadScene("HomeScene");
    }

    private IEnumerator LevelTransition()
    {
        CurrentState = GameState.LevelTransition;

        if (currentLevelIndex >= levels.Length - 1)
        {
            HandleWin();
            yield break;
        }

        if (levelTransitionPanel != null)
        {
            levelTransitionPanel.SetActive(true);
            if (transitionText != null)
                transitionText.text = "Well done! Get ready for the next challenging level...";
        }

        yield return new WaitForSeconds(3f);

        if (levelTransitionPanel != null) levelTransitionPanel.SetActive(false);

        currentLevelIndex++;
        StartCoroutine(StartLevel(currentLevelIndex));
    }

    private void HandleWin()
    {
        CurrentState = GameState.Win;
        if (winPanel != null) winPanel.SetActive(true);
        if (levelText != null) levelText.text = "Level: Completed";

        // NEW: Save stars when winning
        SaveStarsEarned();
    }

    public void ReplayGame()
    {
        if (winPanel != null) winPanel.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);

        score = 0;
        if (scoreText != null) scoreText.text = "0";
        if (finalScoreText != null) finalScoreText.text = "Score: 0";

        currentLevelIndex = 0;
        StartCoroutine(StartLevel(currentLevelIndex));
    }

    public void RestartScene()
    {
        if (tileSpawner != null) tileSpawner.StopSpawning();
        if (musicSource != null) musicSource.Stop();

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void AddScore(int amount)
    {
        if (CurrentState != GameState.Playing) return;

        score += amount;
        if (scoreText != null) scoreText.text = score.ToString();
    }

    // Enhanced Game Over with special effects
    public void GameOver(TileController missedTile = null)
    {
        if (CurrentState != GameState.Playing) return;

        this.missedTile = missedTile;
        CurrentState = GameState.GameOverSequence;

        // Stop spawning and music immediately
        if (tileSpawner != null) tileSpawner.StopSpawning();
        if (musicSource != null) musicSource.Stop();

        // NEW: Save stars earned before game over
        SaveStarsEarned();

        // Start the game over sequence
        StartCoroutine(GameOverSequence());
    }

    // NEW: Save stars to player preferences
    private void SaveStarsEarned()
    {
        if (ProgressStarManager.Instance != null)
        {
            int starsEarned = ProgressStarManager.Instance.GetEarnedStars();
            string currentSongName = musicSource.clip != null ? musicSource.clip.name : "Unknown";

            // Save stars for this song
            int currentBest = PlayerPrefs.GetInt(currentSongName + "_Stars", 0);
            if (starsEarned > currentBest)
            {
                PlayerPrefs.SetInt(currentSongName + "_Stars", starsEarned);
                PlayerPrefs.Save();
                Debug.Log($"Saved {starsEarned} stars for song: {currentSongName}");
            }

            // Save total stars (for overall progression)
            int totalStars = PlayerPrefs.GetInt("TotalStars", 0);
            // We'll update this logic later when we have multiple songs
        }
    }

    public void ResetGreetings()
    {
        if (GreetingsManager.Instance != null)
        {
            GreetingsManager.Instance.ResetTapCount();
        }
    }

    // Game Over Sequence with special effects
    private IEnumerator GameOverSequence()
    {
        // Step 1: Move all tiles up by 2-3 units
        MoveAllTilesUp(2.5f);

        // Step 2: Highlight the missed tile in red and play sound
        if (missedTile != null)
        {
            // Turn the missed tile red
            missedTile.MarkAsMissed();

            // Play the high beep sound
            if (gameOverBeep != null && sfxSource != null)
            {
                sfxSource.PlayOneShot(gameOverBeep);
            }
        }

        // Step 3: Wait for 2 seconds to show the effect
        yield return new WaitForSeconds(2f);

        // Step 4: Show the game over panel
        CurrentState = GameState.GameOver;

        if (finalScoreText != null) finalScoreText.text = "Score: " + score.ToString();
        if (gameOverPanel != null) gameOverPanel.SetActive(true);

        Debug.Log("Game Over triggered (score: " + score + ")");
    }

    // Move all existing tiles up by specified amount
    private void MoveAllTilesUp(float moveAmount)
    {
        // Find all active tiles in the scene
        TileController[] allTiles = FindObjectsOfType<TileController>();

        foreach (TileController tile in allTiles)
        {
            // Move each tile up
            tile.transform.position += Vector3.up * moveAmount;

            // Disable further movement for all tiles
            tile.StopMovement();
        }
    }
}

[System.Serializable]
public class LevelData
{
    public string LevelName;
    public float MusicSpeed;
    public float TileSpeed;

    public LevelData(string name, float musicSpeed, float tileSpeed)
    {
        LevelName = name;
        MusicSpeed = musicSpeed;
        TileSpeed = tileSpeed;
    }
}