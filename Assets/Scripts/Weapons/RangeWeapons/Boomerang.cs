using UnityEngine;

public class Boomerang : RangedWeapon, IFixedUpdateObserver
{
    [SerializeField] private float acceleration;
    private bool isReturning;

    public override void ObserveFixedUpdate()
    {
        if (isReturning)
        {
            velocity += acceleration * Time.fixedDeltaTime * -1;
        }
        else
        {
            velocity -= acceleration * Time.fixedDeltaTime;
            if (velocity <= 0)
            {
                velocity = 0;
                isReturning = true;
            }
        }

        currentLifeTime += Time.fixedDeltaTime;
        rb.linearVelocity = direction.normalized * velocity;

        if (currentLifeTime > maxLifeTime)
        {
            ReturnToPool();
        }
    }
}
