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

        // 1) Uszkodzenia dla płotków
        ApplyDamageToFences(worldPos, damagedFences);

        // 2) Uszkodzenia dla gracza
        ApplyDamageToPlayer(worldPos);

        // Własna eksplozja w centrum
        HandleExplosionAtCell(centerCell, worldPos, damagedFences);

        // Kierunki rozchodzenia się eksplozji
        Vector3Int[] directions = new Vector3Int[]
        {
            Vector3Int.up,
            Vector3Int.down,
            Vector3Int.left,
            Vector3Int.right
        };

        foreach (var dir in directions)
        {
            for (int i = 1; i <= explosionRadius; i++)
            {
                Vector3Int nextCell = centerCell + dir * i;

                if (IsObstacleBetween(centerCell, nextCell)) break;

                Vector3 worldCenter = tilemap.CellToWorld(nextCell) + tilemap.cellSize / 2f;

                // Sprawdź, czy eksplozja została zablokowana przez płotek
                Collider2D[] hits = Physics2D.OverlapCircleAll(worldCenter, 0.4f);
                bool blockedByFence = false;

                foreach (var hit in hits)
                {
                    Fences fence = hit.GetComponent<Fences>();
                    if (fence != null && !damagedFences.Contains(fence))
                    {
                        Vector3 closestBarrelPos = FindClosestBarrel(hit.transform.position);
                        Vector3 damageOrigin = closestBarrelPos != Vector3.zero ? closestBarrelPos : worldPos;
                        float distance = Vector2.Distance(damageOrigin, hit.transform.position);
                        int damage = CalculateDamageByDistance(distance);

                        fence.TakeDamage(damage);
                        damagedFences.Add(fence);

                        Debug.Log($"[BLOCKED] Eksplozja zatrzymana przez płotek {fence.name} (cell: {nextCell}) z obrażeniami {damage}");
                        blockedByFence = true;
                        break;
                    }
                }

                if (blockedByFence)
                    break; // nie rozprzestrzeniaj dalej

                HandleExplosionAtCell(nextCell, worldPos, damagedFences);
            }
        }
    }

    private void HandleExplosionAtCell(Vector3Int cell, Vector3 explosionOrigin, HashSet<Fences> damagedFences)
    {
        Vector3 worldCenter = tilemap.CellToWorld(cell) + tilemap.cellSize / 2f;

        tilemap.SetTile(cell, explosionTile);
        HandleTaggedDestruction(cell, damagedFences);
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

    private bool IsObstacleBetween(Vector3Int fromCell, Vector3Int toCell)
    {
        Vector3 fromWorld = tilemap.CellToWorld(fromCell) + tilemap.cellSize / 2f;
        Vector3 toWorld = tilemap.CellToWorld(toCell) + tilemap.cellSize / 2f;

        Vector3 direction = toWorld - fromWorld;
        float distance = direction.magnitude;
        direction.Normalize();

        return Physics2D.Raycast(fromWorld, direction, distance, obstacleLayers);
    }

    private void ApplyDamageToPlayer(Vector3 explosionCenter)
    {
        // Załóżmy, że gracz ma tag "Player" i komponent PlayerHealth
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        HealthSystem playerHealth = player.GetComponent<HealthSystem>();
        if (playerHealth == null) return;

        float distance = Vector2.Distance(explosionCenter, player.transform.position);

        if (distance > damageRadius) return; // poza zasięgiem obrażeń

        int damage = CalculateDamageByDistance(distance);

        Debug.Log($"[PLAYER DAMAGE] Gracz otrzymuje {damage} obrażeń (odległość od eksplozji: {distance:F2})");

        playerHealth.GetDmg(damage);
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