
/*
using UnityEngine;

public class TileController : MonoBehaviour
{
    bool isHit = false;
    float offscreenY;

    void Start()
    {
        // set a y threshold below which the tile is considered missed
        offscreenY = -Camera.main.orthographicSize - 1f;
    }

    void Update()
    {
        if (GameManager.Instance == null) return;
        if (GameManager.Instance.CurrentState != GameManager.GameState.Playing) return;

        // move down
        transform.Translate(Vector3.down * GameManager.Instance.tileFallSpeed * Time.deltaTime, Space.World);

        // if tile goes below screen without being hit -> miss
        if (!isHit && transform.position.y < offscreenY)
        {
            // missed a tile -> game over
            GameManager.Instance.GameOver();
            Destroy(gameObject);
        }
    }

    // called by InputManager when this tile is tapped
    public void Hit()
    {
        if (isHit) return;
        isHit = true;

        // award points
        GameManager.Instance.AddScore(1);

        // TODO: spawn tap VFX / sound here later

        Destroy(gameObject);
    }
}
*/
using UnityEngine;

public class TileController : MonoBehaviour
{
    bool isHit = false;
    bool hasBeenMissed = false;
    float offscreenY;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private bool canMove = true;

    [Header("Tap Effects")]
    public GameObject musicNoteEffectPrefab;
    public Vector3 noteEffectOffset = new Vector3(0, 0.2f, 0);

    void Start()
    {
        offscreenY = -Camera.main.orthographicSize - 1f;
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    void Update()
    {
        if (GameManager.Instance == null) return;
        if (GameManager.Instance.CurrentState != GameManager.GameState.Playing) return;
        if (!canMove) return;

        transform.Translate(Vector3.down * GameManager.Instance.tileFallSpeed * Time.deltaTime, Space.World);

        if (!isHit && !hasBeenMissed && transform.position.y < offscreenY)
        {
            hasBeenMissed = true;
            Debug.Log($"Tile missed! Position: {transform.position.y}, Threshold: {offscreenY}");
            GameManager.Instance.GameOver(this);
        }

        if (transform.position.y < offscreenY - 2f)
        {
            Destroy(gameObject);
        }
    }

    public void Hit()
    {
        if (isHit) return;
        isHit = true;

        // award points
        GameManager.Instance.AddScore(1);

        // NEW: Register tap for greetings
        if (GreetingsManager.Instance != null)
        {
            GreetingsManager.Instance.RegisterTap();
        }

        // Spawn music note effect
        SpawnMusicNoteEffect();

        Destroy(gameObject);
    }

    private void SpawnMusicNoteEffect()
    {
        if (musicNoteEffectPrefab != null)
        {
            Vector3 spawnPosition = transform.position + noteEffectOffset;
            Instantiate(musicNoteEffectPrefab, spawnPosition, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("Music Note Effect Prefab is not assigned in TileController");
        }
    }

    public void MarkAsMissed()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.red;
        }
    }

    public void StopMovement()
    {
        canMove = false;
    }
}