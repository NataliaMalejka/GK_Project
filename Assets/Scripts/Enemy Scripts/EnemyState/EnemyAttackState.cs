using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : State<Enemy>
{
    private Dictionary<StateType, State<Enemy>> availableStates;
    private ObjectPool<Bullet> bulletPool;
    private float cooldownTimer = 0f;

    public EnemyAttackState(Enemy controller, StateMachine<Enemy> stateMachine, Dictionary<StateType, State<Enemy>> availableStates, ObjectPool<Bullet> bulletPool)
        : base(controller, stateMachine)
    {
        this.availableStates = availableStates;
        this.bulletPool = bulletPool;
    }

    public override void EnterState()
    {
        cooldownTimer = 0f;
    }

    public override void FixedUpdateState()
    {
        if (!controller.seePlayer)
        {
            stateMachine.ChangeState(availableStates[StateType.Idle]);
            return;
        }

        cooldownTimer -= Time.fixedDeltaTime;

        if (cooldownTimer <= 0f)
        {
            FireBullet();
            cooldownTimer = controller.GetComponent<BrownCat>().bullet.cooldown;
        }
    }

    private void FireBullet()
    {
        Bullet bullet = bulletPool.GetObjectFromPool(controller.transform.position);
    }
}
