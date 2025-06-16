using Unity.VisualScripting;
using UnityEngine;

public class Boomerang : RangedWeapon, IFixedUpdateObserver
{
    [SerializeField] private float acceleration;
    private float currentAcceleration;
    private bool isReturning;

    protected override void OnEnable()
    {
        base.OnEnable();
        currentAcceleration = acceleration;
    }

    public override void ObserveFixedUpdate()
    {
        if (isReturning)
        {
            velocity += currentAcceleration * Time.fixedDeltaTime * -1;
        }
        else
        {
            velocity -= currentAcceleration * Time.fixedDeltaTime;
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
            currentAcceleration = acceleration;
            ReturnToPool();
        }
    }
}
