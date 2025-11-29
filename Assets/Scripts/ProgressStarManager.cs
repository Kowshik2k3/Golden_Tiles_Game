using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ProgressStarManager : MonoBehaviour
{
    public static ProgressStarManager Instance;

    [Header("Progress Bar")]
    public Slider progressSlider;
    public Image progressFill;
    public Color easyColor = Color.green;
    public Color mediumColor = Color.yellow;
    public Color hardColor = Color.red;

    [Header("Stars")]
    public Image[] stars;
    public Sprite starEmpty;
    public Sprite starFilled;
    public Color starFilledColor = Color.yellow;

    [Header("Audio")]
    public AudioClip starSound;
    private AudioSource audioSource;

    private int currentStars = 0;
    private int maxStars = 3;
    private float progressPerLevel = 0.333f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }
    }

    private void Start()
    {
        InitializeProgressBar();
    }

    private void InitializeProgressBar()
    {
        if (progressSlider != null)
        {
            progressSlider.value = 0f;
            progressFill.color = easyColor;
        }

        foreach (Image star in stars)
        {
            star.sprite = starEmpty;
            star.color = Color.white;
        }

        currentStars = 0;
    }

    public void UpdateProgress(float progress, int currentLevelIndex)
    {
        if (progressSlider != null)
        {
            float totalProgress = (currentLevelIndex * progressPerLevel) + (progress * progressPerLevel);
            progressSlider.value = totalProgress;

            switch (currentLevelIndex)
            {
                case 0: // Easy
                    progressFill.color = easyColor;
                    break;
                case 1: // Medium
                    progressFill.color = mediumColor;
                    break;
                case 2: // Hard
                    progressFill.color = hardColor;
                    break;
            }
        }
    }

    public void AwardStar(int starIndex)
    {
        if (starIndex >= 0 && starIndex < stars.Length && starIndex >= currentStars)
        {
            stars[starIndex].sprite = starFilled;
            stars[starIndex].color = starFilledColor;
            currentStars = starIndex + 1;

            PlayStarSound();
            StartCoroutine(StarAnimation(stars[starIndex]));
        }
    }

    private void PlayStarSound()
    {
        if (starSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(starSound);
        }
        else
        {
            Debug.LogWarning("Star sound or AudioSource is not assigned in ProgressStarManager");
        }
    }

    private IEnumerator StarAnimation(Image star)
    {
        Vector3 originalScale = star.transform.localScale;
        float duration = 0.5f;
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float scale = Mathf.Lerp(1f, 1.5f, timer / duration);
            star.transform.localScale = originalScale * scale;
            yield return null;
        }

        timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float scale = Mathf.Lerp(1.5f, 1f, timer / duration);
            star.transform.localScale = originalScale * scale;
            yield return null;
        }

        star.transform.localScale = originalScale;
    }

    public int GetEarnedStars()
    {
        return currentStars;
    }

    public void ResetProgress()
    {
        InitializeProgressBar();
    }
}