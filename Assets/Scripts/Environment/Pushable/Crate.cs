using UnityEngine;

public class Crate : DestructibleWithPickup
{
    [Header("Pickup to spawn after destruction")]
    public GameObject pickupPrefab;

    protected override GameObject[] GetPickupPrefabs()
    {
        return pickupPrefab != null ? new GameObject[] { pickupPrefab } : null;
    }
}