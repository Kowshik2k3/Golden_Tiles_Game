/*

using UnityEngine;

public class TileSpawner : MonoBehaviour
{
    public GameObject tilePrefab;
    public float spawnInterval = 1f;
    public int columnCount = 4;
    public float ySpawnPosition = 6f;

    private float[] columnPositions;
    private float tileWidth;
    private bool isSpawning = false;

    private void Start()
    {
        SetupColumns();
        FixTilePrefab(); // Ensure tile has proper rendering
    }

    private void FixTilePrefab()
    {
        // Force the tile prefab to have correct rendering settings
        SpriteRenderer sr = tilePrefab.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.color = Color.white;
            sr.sortingLayerName = "Default";
            sr.sortingOrder = 100;

            // CRITICAL: Ensure material is properly set
            sr.sharedMaterial = new Material(Shader.Find("Sprites/Default"));
        }
    }

    private void SetupColumns()
    {
        // Get tile width
        tileWidth = tilePrefab.GetComponent<SpriteRenderer>().bounds.size.x;

        // Calculate total width taken by columns
        float totalColumnsWidth = (columnCount - 1) * tileWidth;

        // Center align columns
        float startX = -totalColumnsWidth / 2f;

        // Store all X positions for columns
        columnPositions = new float[columnCount];
        for (int i = 0; i < columnCount; i++)
        {
            columnPositions[i] = startX + i * tileWidth;
        }
    }

    private void SpawnTile()
    {
        int randomColumn = Random.Range(0, columnCount);
        float spawnX = columnPositions[randomColumn];
        Vector3 spawnPos = new Vector3(spawnX, ySpawnPosition, 0);

        GameObject tile = Instantiate(tilePrefab, spawnPos, Quaternion.identity);

        // FORCE proper rendering settings on spawned tile
        SpriteRenderer sr = tile.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.color = Color.white;
            sr.sortingLayerName = "Default";
            sr.sortingOrder = 100;

            // 🔥 CRITICAL FIX: Force the material to ensure it's included in build
            sr.material = new Material(Shader.Find("Sprites/Default"));

            Debug.Log($"Tile spawned - Color: {sr.color}, Material: {sr.material.name}");
        }
    }

    public void StartSpawning()
    {
        if (isSpawning) return;
        isSpawning = true;
        InvokeRepeating(nameof(SpawnTile), 0f, spawnInterval);
    }

    public void StopSpawning()
    {
        if (!isSpawning) return;
        isSpawning = false;
        CancelInvoke(nameof(SpawnTile));
    }
}
*/


using UnityEngine;

public class TileSpawner : MonoBehaviour
{
    public GameObject tilePrefab;
    public float spawnInterval = 1f;
    public int columnCount = 4;
    public float ySpawnPosition = 6f;

    private float[] columnPositions;
    private float tileWidth;
    private bool isSpawning = false;

    private void Start()
    {
        SetupColumns();
        FixTilePrefab(); // Ensure tile has proper rendering
    }

    private void FixTilePrefab()
    {
        // Force the tile prefab to have correct rendering settings
        SpriteRenderer sr = tilePrefab.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.color = Color.white;
            sr.sortingLayerName = "Default";
            sr.sortingOrder = 100;

            // CRITICAL: Ensure material is properly set
            sr.sharedMaterial = new Material(Shader.Find("Sprites/Default"));
        }
    }

    private void SetupColumns()
    {
        // Get tile width
        tileWidth = tilePrefab.GetComponent<SpriteRenderer>().bounds.size.x;

        // Calculate total width taken by columns
        float totalColumnsWidth = (columnCount - 1) * tileWidth;

        // Center align columns
        float startX = -totalColumnsWidth / 2f;

        // Store all X positions for columns
        columnPositions = new float[columnCount];
        for (int i = 0; i < columnCount; i++)
        {
            columnPositions[i] = startX + i * tileWidth;
        }
    }

    private void SpawnTile()
    {
        int randomColumn = Random.Range(0, columnCount);
        float spawnX = columnPositions[randomColumn];
        Vector3 spawnPos = new Vector3(spawnX, ySpawnPosition, 0);

        GameObject tile = Instantiate(tilePrefab, spawnPos, Quaternion.identity);

        // FORCE proper rendering settings on spawned tile
        SpriteRenderer sr = tile.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.color = Color.white;
            sr.sortingLayerName = "Default";
            sr.sortingOrder = 100;

            // 🔥 CRITICAL FIX: Force the material to ensure it's included in build
            sr.material = new Material(Shader.Find("Sprites/Default"));

            Debug.Log($"Tile spawned - Color: {sr.color}, Material: {sr.material.name}");
        }
    }

    public void StartSpawning()
    {
        if (isSpawning) return;
        isSpawning = true;
        InvokeRepeating(nameof(SpawnTile), 0f, spawnInterval);
    }

    public void StopSpawning()
    {
        if (!isSpawning) return;
        isSpawning = false;
        CancelInvoke(nameof(SpawnTile));
    }
}