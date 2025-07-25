using UnityEngine;

public class Pot : DestructibleWithPickup
{
    [Header("Pickups to spawn after destruction")]
    public GameObject[] pickupPrefabs;

    protected override GameObject[] GetPickupPrefabs()
    {
        return pickupPrefabs;
    }
}