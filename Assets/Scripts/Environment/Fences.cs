using UnityEngine;
using UnityEngine.Tilemaps;

/** 
 * 
 * @author Krzysztof Gach
 * @version 1.0
 */
public class Fences : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 3;
    public int currentHealth;

    [Header("Visual Settings")]
    public Sprite[] damageSprites; // 0 = zdrowy, 1 = lekko uszkodzony, 2 = poważnie uszkodzony

    [Header("Collision Settings")]
    public float[] radiusAtHealth; // radius dla każdego poziomu zdrowia [3hp, 2hp, 1hp]

    private SpriteRenderer spriteRenderer;
    private CircleCollider2D circleCollider;

    public int CurrentHealth { get; private set; }

    public void SetHealth(int newHealth)
    {
        CurrentHealth = Mathf.Clamp(newHealth, 0, 3);
    }

    private void Awake()
    {
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
        circleCollider = GetComponent<CircleCollider2D>();

        // Domyślne wartości radiusów jeśli nie zostały ustawione
        if (radiusAtHealth == null || radiusAtHealth.Length == 0)
        {
            radiusAtHealth = new float[] { 0.5f, 0.3f, 0.15f }; // pełne zdrowie, uszkodzony, bardzo uszkodzony
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
            Debug.Log($"Fence {gameObject.name}: Destroyed!");
            Destroy(gameObject);
        }
    }

    private void UpdateSpriteAndRadius()
    {
        // Aktualizuj sprite na podstawie zdrowia
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

        // Aktualizuj radius kolizji na podstawie zdrowia
        UpdateCollisionRadius();
    }

    private void UpdateCollisionRadius()
    {
        if (circleCollider == null) return;

        // Ustaw radius na podstawie aktualnego zdrowia
        if (currentHealth == maxHealth && radiusAtHealth.Length > 0)
        {
            circleCollider.radius = radiusAtHealth[0]; // pełne zdrowie
        }
        else if (currentHealth == 2 && radiusAtHealth.Length > 1)
        {
            circleCollider.radius = radiusAtHealth[1]; // uszkodzony
        }
        else if (currentHealth == 1 && radiusAtHealth.Length > 2)
        {
            circleCollider.radius = radiusAtHealth[2]; // bardzo uszkodzony
        }
        else if (currentHealth > 0)
        {
            // Fallback - użyj ostatniego dostępnego radiusu
            circleCollider.radius = radiusAtHealth[radiusAtHealth.Length - 1];
        }
    }

    // Pomocnicza metoda do debugowania - pokaż aktualny radius w Scene View
    private void OnDrawGizmosSelected()
    {
        if (circleCollider != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, circleCollider.radius);
        }
    }

    // Opcjonalna metoda do ręcznego ustawienia zdrowia (do testowania)
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