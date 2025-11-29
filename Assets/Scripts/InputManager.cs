/*

using UnityEngine;
public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else
        {
            Debug.LogWarning($"Duplicate InputManager ({name}) destroyed.");
            Destroy(gameObject);
            return;
        }
    }

    void Update()
    {
        if (GameManager.Instance == null) return;
        if (GameManager.Instance.CurrentState != GameManager.GameState.Playing) return;

        // Process at most one input per frame to avoid duplicate handling
        bool processed = false;

        // Mouse (Editor / PC)
        if (Input.GetMouseButtonDown(0))
        {
            ProcessTouch(Input.mousePosition);
            processed = true;
        }

        // Touch (Mobile) - only if mouse wasn't already handled this frame
        if (!processed && Input.touchCount > 0)
        {
            Touch t = Input.GetTouch(0);
            if (t.phase == TouchPhase.Began)
            {
                ProcessTouch(t.position);
            }
        }
    }

    void ProcessTouch(Vector2 screenPos)
    {
        Camera cam = Camera.main;
        if (cam == null) return;

        // Use camera distance so ScreenToWorldPoint is correct for both ortho & perspective
        float zDist = Mathf.Abs(cam.transform.position.z);
        Vector3 worldPoint = cam.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, zDist));
        Vector2 touchPos = new Vector2(worldPoint.x, worldPoint.y);

        // Single-point 2D raycast (reliable for 2D tap detection)
        RaycastHit2D hit = Physics2D.Raycast(touchPos, Vector2.zero);

        if (hit.collider != null)
        {
            TileController tile = hit.collider.GetComponentInParent<TileController>();
            if (tile != null)
            {
                Debug.Log($"Tapped tile → Hit registered: {tile.name} (collider: {hit.collider.name})");
                tile.Hit();
                return;
            }
            else
            {
                Debug.Log($"Hit non-tile collider: {hit.collider.name}");
            }
        }

        Debug.Log("No tile hit → GameOver");
        GameManager.Instance.GameOver();
    }
}
*/

using UnityEngine;
public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    [Header("Tap Effects")]
    public GameObject musicNoteEffectPrefab; // Fallback if tile doesn't have one

    void Awake()
    {
        if (Instance == null) Instance = this;
        else
        {
            Debug.LogWarning($"Duplicate InputManager ({name}) destroyed.");
            Destroy(gameObject);
            return;
        }
    }

    void Update()
    {
        if (GameManager.Instance == null) return;
        // Don't process input during game over sequence
        if (GameManager.Instance.CurrentState != GameManager.GameState.Playing) return;

        // Process at most one input per frame to avoid duplicate handling
        bool processed = false;

        // Mouse (Editor / PC)
        if (Input.GetMouseButtonDown(0))
        {
            ProcessTouch(Input.mousePosition);
            processed = true;
        }

        // Touch (Mobile) - only if mouse wasn't already handled this frame
        if (!processed && Input.touchCount > 0)
        {
            Touch t = Input.GetTouch(0);
            if (t.phase == TouchPhase.Began)
            {
                ProcessTouch(t.position);
            }
        }
    }

    void ProcessTouch(Vector2 screenPos)
    {
        Camera cam = Camera.main;
        if (cam == null) return;

        float zDist = Mathf.Abs(cam.transform.position.z);
        Vector3 worldPoint = cam.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, zDist));
        Vector2 touchPos = new Vector2(worldPoint.x, worldPoint.y);

        RaycastHit2D hit = Physics2D.Raycast(touchPos, Vector2.zero);

        if (hit.collider != null)
        {
            TileController tile = hit.collider.GetComponentInParent<TileController>();
            if (tile != null)
            {
                Debug.Log($"Tapped tile → Hit registered: {tile.name} (collider: {hit.collider.name})");
                tile.Hit();
                return;
            }
            else
            {
                Debug.Log($"Hit non-tile collider: {hit.collider.name}");

                // NEW: Optional - spawn music note even when tapping non-tile objects
                // SpawnMusicNoteEffect(worldPoint);
            }
        }

        // CHANGED: No longer calls GameOver() when clicking outside tiles
        Debug.Log("Clicked outside tiles - Game continues");

        // NEW: Optional - spawn music note even when tapping empty space
        // SpawnMusicNoteEffect(worldPoint);
    }

    // NEW: Helper method to spawn music note effect at world position
    private void SpawnMusicNoteEffect(Vector3 worldPosition)
    {
        if (musicNoteEffectPrefab != null)
        {
            Instantiate(musicNoteEffectPrefab, worldPosition, Quaternion.identity);
        }
    }
}