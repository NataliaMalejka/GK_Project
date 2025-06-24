using Unity.VisualScripting;
using UnityEngine;

public class BulletShield : MonoBehaviour
{
    private int dmg = 2;
    private int damageDuration = 1; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamageable damageable = collision.GetComponent<IDamageable>();

        if (damageable != null)
        {
            damageable.ReduceHP(dmg, damageDuration);
        }
    }
}
