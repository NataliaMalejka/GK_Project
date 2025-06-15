using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] protected int dmg;
    [SerializeField] protected int damageDuration;
    public float cooldown;

    protected GameObject controller;

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
