using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] protected int dmg;
    public float neededStamina;
    public int neededMana;
    public float cooldown;

    protected SpriteRenderer spriteRenderer;

    protected virtual void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>() ?? gameObject.AddComponent<SpriteRenderer>();
    }
}
