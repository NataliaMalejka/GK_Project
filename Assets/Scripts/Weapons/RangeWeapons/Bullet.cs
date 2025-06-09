using Unity.VisualScripting;
using UnityEngine;

public class Bullet : RangedWeapon, IFixedUpdateObserver
{
    public override void ObserveFixedUpdate()
    {
        currentLifeTime += Time.fixedDeltaTime;
        rb.linearVelocity = direction.normalized * velocity;

        if (currentLifeTime > maxLifeTime)
        {
            ReturnToPool();
        }
    }
}
