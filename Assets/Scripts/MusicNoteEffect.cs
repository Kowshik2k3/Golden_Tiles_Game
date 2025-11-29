using UnityEngine;
using System.Collections;

public class MusicNoteEffect : MonoBehaviour
{
    [Header("Music Note Settings")]
    public float floatSpeed = 2f;
    public float floatDistance = 1.5f;
    public float fadeDuration = 0.5f;

    private Vector3 startPosition;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    void Start()
    {
        startPosition = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }

        // Start the floating and fade effect
        StartCoroutine(FloatAndFade());
    }

    IEnumerator FloatAndFade()
    {
        float timer = 0f;
        Vector3 targetPosition = startPosition + Vector3.up * floatDistance;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;

            // Float upward
            transform.position = Vector3.Lerp(startPosition, targetPosition, timer / fadeDuration);

            // Fade out
            if (spriteRenderer != null)
            {
                float alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);
                spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            }

            yield return null;
        }

        // Destroy when done
        Destroy(gameObject);
    }
}