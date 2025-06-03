using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class ExplosionTilemapController : MonoBehaviour
{
    public Tilemap tilemap;
    public TileBase explosionTile;
    public float explosionRadius = 3f;
    public LayerMask obstacleLayers;

    [Header("Damage Settings")]
    public int maxDamage = 3;
    public float damageRadius = 3f;

    [Header("Damage Zones")]
    public float instantKillRadius = 0.5f;
    public float heavyDamageRadius = 1.5f;
    public int instantKillDamage = 3;
    public int heavyDamage = 2;
    public int lightDamage = 1;

    [Header("Barrel Search Settings")]
    public float barrelSearchRadius = 10f; // Promień w którym szukamy beczek

    public void ExplodeAt(Vector3 worldPos)
    {
        // Stwórz hashset tylko raz dla całego łańcucha eksplozji
        HashSet<Fences> damagedFences = new HashSet<Fences>();
        ExplodeInternal(worldPos, damagedFences);
    }

    private void ExplodeInternal(Vector3 worldPos, HashSet<Fences> damagedFences)
    {
        Vector3Int centerCell = tilemap.WorldToCell(worldPos);
        int radiusInCells = Mathf.CeilToInt(explosionRadius);
        
        ApplyDamageToFences(worldPos, damagedFences);

        for (int x = -radiusInCells; x <= radiusInCells; x++)
        {
            for (int y = -radiusInCells; y <= radiusInCells; y++)
            {
                Vector3Int checkCell = centerCell + new Vector3Int(x, y, 0);
                float dist = Vector2.Distance(
                    new Vector2(centerCell.x, centerCell.y),
                    new Vector2(checkCell.x, checkCell.y)
                );

                if (dist <= explosionRadius && !IsObstacleBetween(centerCell, checkCell))
                {
                    Vector3 worldCenter = tilemap.CellToWorld(checkCell) + tilemap.cellSize / 2f;
                    bool skipExplosionTile = false;

                    // Sprawdź czy jest Fence w tej komórce
                    Collider2D[] hits = Physics2D.OverlapCircleAll(worldCenter, 0.4f);
                    foreach (var hit in hits)
                    {
                        Fences fence = hit.GetComponent<Fences>();
                        if (fence != null)
                        {
                            // Znajdź najbliższą beczkę dla tego płotka
                            Vector3 closestBarrelPos = FindClosestBarrel(hit.transform.position);

                            // Oblicz odległość od najbliższej beczki (lub od centrum wybuchu jeśli brak beczek)
                            Vector3 damageOrigin = closestBarrelPos != Vector3.zero ? closestBarrelPos : worldPos;
                            float distance = Vector2.Distance(damageOrigin, hit.transform.position);
                            int projectedDamage = CalculateDamageByDistance(distance);
                            Debug.Log($"[CELL CHECK] Płotek {fence.name} w komórce {checkCell} - przewidywane obrażenia: {projectedDamage} (odległość: {distance:F2})");
                        }
                    }

                    HandleTaggedDestruction(checkCell, damagedFences);

                    if (!skipExplosionTile)
                    {
                        tilemap.SetTile(checkCell, explosionTile);
                    }
                }
            }
        }
    }

    private Vector3 FindClosestBarrel(Vector3 fencePosition)
    {
        Collider2D[] barrels = Physics2D.OverlapCircleAll(fencePosition, barrelSearchRadius);
        float closestDistance = float.MaxValue;
        Vector3 closestBarrelPosition = Vector3.zero;

        foreach (var barrel in barrels)
        {
            if (barrel.CompareTag("Barrel"))
            {
                float distance = Vector2.Distance(fencePosition, barrel.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestBarrelPosition = barrel.transform.position;
                }
            }
        }

        // Logowanie wyniku
        if (closestBarrelPosition != Vector3.zero)
        {
            Debug.Log($"[FENCE DAMAGE] Płotek na pozycji {fencePosition} -> najbliższa beczka na {closestBarrelPosition} (odległość: {closestDistance:F2})");
        }
        else
        {
            Debug.Log($"[FENCE DAMAGE] Płotek na pozycji {fencePosition} -> brak beczek w promieniu {barrelSearchRadius}");
        }

        return closestBarrelPosition;
    }

    private int CalculateDamageByDistance(float distance)
    {
        if (distance <= instantKillRadius)
        {
            return instantKillDamage;
        }
        else if (distance <= heavyDamageRadius)
        {
            return heavyDamage;
        }
        else
        {
            return lightDamage;
        }
    }

    private void HandleTaggedDestruction(Vector3Int cell, HashSet<Fences> damagedFences)
    {
        Vector3 worldCenter = tilemap.CellToWorld(cell) + tilemap.cellSize / 2f;
        Collider2D[] hits = Physics2D.OverlapCircleAll(worldCenter, 0.4f);

        foreach (var hit in hits)
        {
            GameObject obj = hit.gameObject;

            if (obj.CompareTag("Barrel"))
            {
                Vector3 barrelPosition = obj.transform.position;

                // Znajdź najbliższą beczkę względem tego centrum (dla chain reaction)
                Collider2D[] allBarrels = Physics2D.OverlapCircleAll(barrelPosition, 1f);
                float closestDistance = float.MaxValue;
                Vector3 explosionOrigin = barrelPosition;

                foreach (var other in allBarrels)
                {
                    if (other.CompareTag("Barrel"))
                    {
                        float dist = Vector2.Distance(worldCenter, other.transform.position);
                        if (dist < closestDistance)
                        {
                            closestDistance = dist;
                            explosionOrigin = other.transform.position;
                        }
                    }
                }

                Vector3Int objCell = tilemap.WorldToCell(barrelPosition);
                if (tilemap.GetTile(objCell) != explosionTile)
                {
                    Destroy(obj);
                    ExplodeInternal(explosionOrigin, damagedFences); // użyj najbliższej baryłki jako epicentrum
                }
            }
            else if (obj.CompareTag("Pot"))
            {
                Destroy(obj);
            }
            else if (obj.CompareTag("Crate"))
            {
                Crate crate = obj.GetComponent<Crate>();
                if (crate != null)
                {
                    Debug.Log("Destroying crate via Crate script");
                    crate.DestroySelf();
                }
                else
                {
                    Debug.LogWarning("No Crate component found on object with tag 'Crate'");
                    Destroy(obj);
                }
            }
        }
    }

    private void ApplyDamageToFences(Vector3 explosionCenter, HashSet<Fences> damagedFences)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(explosionCenter, damageRadius);

        foreach (var hit in hits)
        {
            Fences fence = hit.GetComponent<Fences>();
            if (fence != null && !damagedFences.Contains(fence))
            {
                // Znajdź najbliższą beczkę dla tego płotka
                Vector3 closestBarrelPos = FindClosestBarrel(hit.transform.position);
                
                // Oblicz odległość od najbliższej beczki (lub od centrum wybuchu jeśli brak beczek)
                Vector3 damageOrigin = closestBarrelPos != Vector3.zero ? closestBarrelPos : explosionCenter;
                float distance = Vector2.Distance(damageOrigin, hit.transform.position);

                Debug.Log($"[DAMAGE CALC] Płotek {hit.name} otrzyma obrażenia na podstawie odległości {distance:F2} od {(closestBarrelPos != Vector3.zero ? "beczki" : "centrum wybuchu")}");

                if (distance <= instantKillRadius)
                {
                    Debug.Log($"[INSTANT KILL] Płotek {fence.name} zniszczony natychmiastowo (odległość: {distance:F2} <= {instantKillRadius})");
                    Destroy(fence.gameObject);
                    damagedFences.Add(fence);
                }
                else
                {
                    int damage = CalculateDamageByDistance(distance);
                    if (damage > 0)
                    {
                        Debug.Log($"[DAMAGE APPLIED] Płotek {fence.name} otrzymuje {damage} obrażeń (odległość: {distance:F2})");
                        fence.TakeDamage(damage);
                        damagedFences.Add(fence);
                    }
                    else
                    {
                        Debug.Log($"[NO DAMAGE] Płotek {fence.name} poza zasięgiem obrażeń (odległość: {distance:F2})");
                    }
                }
            }
        }
    }

    private bool IsObstacleBetween(Vector3Int fromCell, Vector3Int toCell)
    {
        Vector3 fromWorld = tilemap.CellToWorld(fromCell) + tilemap.cellSize / 2f;
        Vector3 toWorld = tilemap.CellToWorld(toCell) + tilemap.cellSize / 2f;

        Vector3 direction = toWorld - fromWorld;
        float distance = direction.magnitude;
        direction.Normalize();

        return Physics2D.Raycast(fromWorld, direction, distance, obstacleLayers);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, instantKillRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, heavyDamageRadius);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, damageRadius);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, barrelSearchRadius);
    }
}