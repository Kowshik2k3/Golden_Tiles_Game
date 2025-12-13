using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class ButtonBounce : MonoBehaviour,
    IPointerDownHandler,
    IPointerUpHandler
{
    [Header("Press Bounce")]
    public float pressedScale = 1.12f;
    public float bounceDuration = 0.12f;

    [Header("Idle Pulse (Loop)")]
    public bool enableIdlePulse = true;
    public float idleMinScale = 0.95f;
    public float idleMaxScale = 1.05f;
    public float idlePulseSpeed = 1.5f;

    private RectTransform rect;
    private Vector3 originalScale;
    private Coroutine bounceRoutine;
    private Coroutine idleRoutine;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        originalScale = rect.localScale;
    }

    void OnEnable()
    {
        if (enableIdlePulse)
            idleRoutine = StartCoroutine(IdlePulse());
    }

    void OnDisable()
    {
        if (idleRoutine != null)
            StopCoroutine(idleRoutine);

        rect.localScale = originalScale;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        StopIdle();
        StartBounce(pressedScale);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        StartBounce(1f);
        ResumeIdle();
    }

    void StartBounce(float targetScale)
    {
        if (bounceRoutine != null)
            StopCoroutine(bounceRoutine);

        bounceRoutine = StartCoroutine(ScaleRoutine(targetScale));
    }

    IEnumerator ScaleRoutine(float target)
    {
        Vector3 start = rect.localScale;
        Vector3 end = originalScale * target;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.unscaledDeltaTime / bounceDuration;
            rect.localScale = Vector3.Lerp(start, end, EaseOutBack(t));
            yield return null;
        }

        rect.localScale = end;
    }

    IEnumerator IdlePulse()
    {
        float time = 0f;
        while (true)
        {
            time += Time.unscaledDeltaTime * idlePulseSpeed;
            float scale = Mathf.Lerp(idleMinScale, idleMaxScale, (Mathf.Sin(time) + 1f) / 2f);
            rect.localScale = originalScale * scale;
            yield return null;
        }
    }

    void StopIdle()
    {
        if (idleRoutine != null)
        {
            StopCoroutine(idleRoutine);
            idleRoutine = null;
        }
    }

    void ResumeIdle()
    {
        if (enableIdlePulse && idleRoutine == null)
            idleRoutine = StartCoroutine(IdlePulse());
    }

    float EaseOutBack(float x)
    {
        const float c1 = 1.70158f;
        const float c3 = c1 + 1f;
        return 1f + c3 * Mathf.Pow(x - 1f, 3) + c1 * Mathf.Pow(x - 1f, 2);
    }
}
