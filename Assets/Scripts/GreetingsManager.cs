using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class GreetingsManager : MonoBehaviour
{
    public static GreetingsManager Instance;

    [Header("Greetings UI")]
    public TextMeshProUGUI greetingsText;
    public GameObject greetingsPanel;
    public float displayTime = 1f;
    public float fadeDuration = 0.3f;

    [Header("Greetings Messages")]
    public string[] greetingsMessages = {
        "Cool!", "Nice!", "Great!", "Awesome!", "Perfect!",
        "Excellent!", "Amazing!", "Fantastic!", "Incredible!", "Unbelievable!"
    };

    private int tapCount = 0;
    private int showAfterTaps = 2; // Show greeting every 2 taps
    private Coroutine currentGreetingCoroutine;

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
    }

    private void Start()
    {
        // Hide greetings panel initially
        if (greetingsPanel != null)
            greetingsPanel.SetActive(false);
    }

    public void RegisterTap()
    {
        tapCount++;

        // Show greeting every specified number of taps
        if (tapCount % showAfterTaps == 0)
        {
            ShowRandomGreeting();
        }
    }

    private void ShowRandomGreeting()
    {
        if (greetingsMessages.Length == 0) return;

        // Get random message
        string randomMessage = greetingsMessages[Random.Range(0, greetingsMessages.Length)];

        // Show the greeting
        if (currentGreetingCoroutine != null)
            StopCoroutine(currentGreetingCoroutine);

        currentGreetingCoroutine = StartCoroutine(ShowGreetingCoroutine(randomMessage));
    }

    private IEnumerator ShowGreetingCoroutine(string message)
    {
        if (greetingsText != null && greetingsPanel != null)
        {
            // Set message and show panel
            greetingsText.text = message;
            greetingsPanel.SetActive(true);

            // Fade in effect
            CanvasGroup canvasGroup = greetingsPanel.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
                canvasGroup = greetingsPanel.AddComponent<CanvasGroup>();

            canvasGroup.alpha = 0f;

            // Fade in
            float fadeTimer = 0f;
            while (fadeTimer < fadeDuration)
            {
                fadeTimer += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(0f, 1f, fadeTimer / fadeDuration);
                yield return null;
            }

            canvasGroup.alpha = 1f;

            // Wait for display time
            yield return new WaitForSeconds(displayTime);

            // Fade out
            fadeTimer = 0f;
            while (fadeTimer < fadeDuration)
            {
                fadeTimer += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(1f, 0f, fadeTimer / fadeDuration);
                yield return null;
            }

            canvasGroup.alpha = 0f;
            greetingsPanel.SetActive(false);
        }
    }

    public void ResetTapCount()
    {
        tapCount = 0;
    }
}