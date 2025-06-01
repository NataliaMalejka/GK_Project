using UnityEngine;

public class Pot : MonoBehaviour
{
    [Header("Pickups to spawn after destruction")]
    [Tooltip("List of pickup prefabs to spawn")]
    public GameObject[] pickupPrefabs;

    [Tooltip("Number of pickups to spawn (randomly selected from list)")]
    public int pickupsToSpawn = 1;

    [Tooltip("Offset radius to spread pickups slightly")]
    public float spawnRadius = 0.5f;

    private bool isDestroyed = false;

    public void DestroySelf()
    {
        if (isDestroyed) return;
        isDestroyed = true;

        Debug.Log("DestroySelf called on " + name);

        if (pickupPrefabs != null && pickupPrefabs.Length > 0)
        {
            for (int i = 0; i < pickupsToSpawn; i++)
            {
                GameObject prefab = pickupPrefabs[Random.Range(0, pickupPrefabs.Length)];

                Vector3 offset = new Vector3(
                    Random.Range(-spawnRadius, spawnRadius),
                    Random.Range(-spawnRadius, spawnRadius),
                    0f
                );

                Vector3 spawnPos = transform.position + offset;

                Debug.Log($"Spawning pickup {prefab.name} at {spawnPos}");
                Instantiate(prefab, spawnPos, Quaternion.identity);
            }
        }
        else
        {
            Debug.LogWarning("No pickup prefabs assigned on " + name);
        }

        Destroy(gameObject);
    }
}
