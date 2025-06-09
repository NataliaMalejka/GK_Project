using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] protected int dmg;
    [SerializeField] protected int damageDuration;
    public float neededStamina;
    public int neededMana;
    public float cooldown;

    protected SpriteRenderer spriteRenderer;
    protected GameObject controller;

    protected virtual void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>() ?? gameObject.AddComponent<SpriteRenderer>();
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        IDamageable damageable = collision.GetComponent<IDamageable>();

        if (damageable != null && collision.gameObject != controller)
        {
            damageable.ReduceHP(dmg, damageDuration);

            RangedWeapon weapon = this.gameObject.GetComponent<RangedWeapon>();

            if (weapon != null)
            {
                weapon.ReturnToPool();
            }
        }
    }
}
