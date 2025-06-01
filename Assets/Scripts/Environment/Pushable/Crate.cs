using UnityEngine;

public class Crate : MonoBehaviour
{
    [Header("Pickup to spawn after destruction")]
    public GameObject pickupPrefab;

    [Tooltip("Optional offset for pickup spawn position")]
    public Vector3 spawnOffset = Vector3.zero;

    private bool isDestroyed = false;

    // Call this method to destroy the crate
    public void DestroySelf()
    {
        if (isDestroyed) return;
        isDestroyed = true;

        Debug.Log("DestroySelf called on " + name);

        if (pickupPrefab != null)
        {
            Vector3 spawnPos = transform.position + spawnOffset;
            Debug.Log("Spawning pickup at " + spawnPos);
            Instantiate(pickupPrefab, spawnPos, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("pickupPrefab is not assigned on " + name);
        }

        Destroy(gameObject);
    }

}
