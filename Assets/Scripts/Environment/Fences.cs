using UnityEngine;

/**
 * Handles fence health, visuals, collider radius, and explosion damage.
 * 
 * @author Krzysztof Gach
 * @version 1.2
 */
public class Fences : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 3;
    public int currentHealth;

    [Header("Destroy Settings")]
    public bool destroyOnZeroHealth = true;

    [Header("Visual Settings")]
    public Sprite[] damageSprites; // 0 = healthy, 1 = medium, 2 = low health

    [Header("Collision Settings")]
    public float[] radiusAtHealth; // [3hp, 2hp, 1hp]

    private SpriteRenderer spriteRenderer;
    private CircleCollider2D circleCollider;

    public int CurrentHealth { get; private set; }

    private void Awake()
    {
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
        circleCollider = GetComponent<CircleCollider2D>();

        if (radiusAtHealth == null || radiusAtHealth.Length == 0)
        {
            radiusAtHealth = new float[] { 0.5f, 0.3f, 0.15f };
        }

        UpdateSpriteAndRadius();
    }

    public void TakeDamage(int damage)
    {
        Debug.Log($"Fence {gameObject.name}: Taking {damage} damage. Health before: {currentHealth}");

        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);

        Debug.Log($"Fence {gameObject.name}: Health after: {currentHealth}");

        UpdateSpriteAndRadius();

        if (currentHealth <= 0)
        {
            if (destroyOnZeroHealth)
            {
                Debug.Log($"Fence {gameObject.name}: Destroyed!");
                Destroy(gameObject);
            }
            else
            {
                Debug.Log($"Fence {gameObject.name}: Disabled (not destroyed).");
                if (circleCollider != null)
                    circleCollider.enabled = false;

                if (spriteRenderer != null)
                    spriteRenderer.color = new Color(1f, 1f, 1f, 0.4f); // semi-transparent
            }
        }
    }

    public void ApplyExplosionDamage(Vector3 explosionCenter, float damageRadius, float instantKillRadius, Vector3[] barrelPositions)
    {
        Vector3 closestBarrel = FindClosestBarrel(barrelPositions);
        Vector3 damageOrigin = closestBarrel != Vector3.zero ? closestBarrel : explosionCenter;
        float distance = Vector2.Distance(damageOrigin, transform.position);

        if (distance > damageRadius)
        {
            Debug.Log($"[NO DAMAGE] {name} is outside the damage radius.");
            return;
        }

        if (distance <= instantKillRadius)
        {
            Debug.Log($"[INSTANT KILL] {name} destroyed instantly (distance: {distance:F2} <= {instantKillRadius})");
            Destroy(gameObject);
            return;
        }

        int damage = CalculateDamageByDistance(distance, damageRadius);
        if (damage > 0)
        {
            Debug.Log($"[DAMAGE APPLIED] {name} takes {damage} damage (distance: {distance:F2})");
            TakeDamage(damage);
        }
        else
        {
            Debug.Log($"[NO DAMAGE] {name} within radius but damage is 0 (distance: {distance:F2})");
        }
    }

    private Vector3 FindClosestBarrel(Vector3[] barrelPositions)
    {
        Vector3 closest = Vector3.zero;
        float minDist = float.MaxValue;

        foreach (var pos in barrelPositions)
        {
            float dist = Vector2.Distance(pos, transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = pos;
            }
        }

        return closest;
    }

    private int CalculateDamageByDistance(float distance, float maxRadius)
    {
        float normalized = 1f - (distance / maxRadius);
        return Mathf.CeilToInt(normalized * maxHealth); // e.g. maxHealth = max damage
    }

    private void UpdateSpriteAndRadius()
    {
        if (currentHealth == maxHealth && damageSprites.Length > 0)
        {
            spriteRenderer.sprite = damageSprites[0];
        }
        else if (currentHealth == 2 && damageSprites.Length > 1)
        {
            spriteRenderer.sprite = damageSprites[1];
        }
        else if (currentHealth == 1 && damageSprites.Length > 2)
        {
            spriteRenderer.sprite = damageSprites[2];
        }

        UpdateCollisionRadius();
    }

    private void UpdateCollisionRadius()
    {
        if (circleCollider == null) return;

        if (currentHealth == maxHealth && radiusAtHealth.Length > 0)
        {
            circleCollider.radius = radiusAtHealth[0];
        }
        else if (currentHealth == 2 && radiusAtHealth.Length > 1)
        {
            circleCollider.radius = radiusAtHealth[1];
        }
        else if (currentHealth == 1 && radiusAtHealth.Length > 2)
        {
            circleCollider.radius = radiusAtHealth[2];
        }
        else if (currentHealth > 0)
        {
            circleCollider.radius = radiusAtHealth[radiusAtHealth.Length - 1];
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (circleCollider != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, circleCollider.radius);
        }
    }

    // Context menu utilities for testing
    [ContextMenu("Set Health to 1")]
    public void SetHealthToOne()
    {
        currentHealth = 1;
        UpdateSpriteAndRadius();
    }

    [ContextMenu("Set Health to 2")]
    public void SetHealthToTwo()
    {
        currentHealth = 2;
        UpdateSpriteAndRadius();
    }

    [ContextMenu("Restore Full Health")]
    public void RestoreFullHealth()
    {
        currentHealth = maxHealth;
        UpdateSpriteAndRadius();
    }
}
