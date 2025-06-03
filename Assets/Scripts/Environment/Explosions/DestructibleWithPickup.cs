using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class DestructibleWithPickup : MonoBehaviour, IDestructible
{
    [Header("Pickup Settings")]
    public float spawnRadius = 0.5f;
    public int pickupsToSpawn = 1;
    public LayerMask spawnCollisionMask;

    [Header("Tilemap Settings")]
    public Tilemap walkableTilemap;

    private bool isDestroyed = false;

    public virtual void DestroySelf()
    {
        if (isDestroyed) return;
        isDestroyed = true;

        GameObject[] prefabs = GetPickupPrefabs();
        if (prefabs == null || prefabs.Length == 0)
        {
            Debug.LogWarning("No pickup prefabs assigned on " + name);
        }
        else
        {
            for (int i = 0; i < pickupsToSpawn; i++)
            {
                GameObject prefab = prefabs[Random.Range(0, prefabs.Length)];
                Vector3 spawnPos = FindSpawnPosition();
                Instantiate(prefab, spawnPos, Quaternion.identity);
            }
        }

        Destroy(gameObject);
    }

    protected abstract GameObject[] GetPickupPrefabs();

    private Vector3 FindSpawnPosition()
    {
        Vector3 origin = transform.position;

        for (int attempt = 0; attempt < 20; attempt++)
        {
            Vector3 offset = new Vector3(
                Random.Range(-spawnRadius, spawnRadius),
                Random.Range(-spawnRadius, spawnRadius),
                0f
            );
            Vector3 candidate = origin + offset;

            if (IsPositionValid(candidate))
                return candidate;
        }

        float searchRadius = spawnRadius + 0.5f;
        float maxSearchRadius = spawnRadius + 5f;
        float step = 0.5f;

        while (searchRadius <= maxSearchRadius)
        {
            int pointsToCheck = Mathf.CeilToInt(2 * Mathf.PI * searchRadius / step);
            for (int i = 0; i < pointsToCheck; i++)
            {
                float angle = (i / (float)pointsToCheck) * 2 * Mathf.PI;
                Vector3 candidate = origin + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f) * searchRadius;

                if (IsPositionValid(candidate))
                    return candidate;
            }
            searchRadius += step;
        }

        return origin;
    }

    private bool IsPositionValid(Vector3 position)
    {
        Vector3Int gridPos = walkableTilemap.WorldToCell(position);
        if (walkableTilemap.GetTile(gridPos) == null)
            return false;

        if (Physics2D.OverlapCircle(position, 0.2f, spawnCollisionMask))
            return false;

        return true;
    }
}
