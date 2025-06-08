using UnityEngine;

public class FireBall : RangedWeapon, IFixedUpdateObserver
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
