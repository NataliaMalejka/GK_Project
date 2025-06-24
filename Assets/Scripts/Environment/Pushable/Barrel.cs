using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Barrel : DestructibleWithPickup, IExplodable
{
    [Header("Explosion Settings")]
    public GameObject explosionPrefab;

    [Header("Damage Settings")]
    public float damageRadius = 5f;
    public float instantKillRadius = 1f;

    public override void DestroySelf()
    {
        // fallback in case DestroySelf is called manually
        Explode(transform.position, new HashSet<Fences>());
    }

    public void Explode(Vector3 explosionCenter, HashSet<Fences> damagedFences)
    {
        SoundsManager.Instance.PlayAudioClip(Sounds.Explosion);
        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, explosionCenter, Quaternion.identity);
        }

        Collider2D[] hits = Physics2D.OverlapCircleAll(explosionCenter, damageRadius);

        Vector3[] barrelPositions = GameObject.FindGameObjectsWithTag("Barrel")
                                            .Select(b => b.transform.position)
                                            .ToArray();

        foreach (var hit in hits)
        {
            Fences fence = hit.GetComponent<Fences>();
            if (fence != null && !damagedFences.Contains(fence))
            {
                fence.ApplyExplosionDamage(explosionCenter, damageRadius, instantKillRadius, barrelPositions);
                damagedFences.Add(fence);
            }
        }

        Destroy(gameObject);
    }

    protected override GameObject[] GetPickupPrefabs()
    {
        return null;
    }
}
