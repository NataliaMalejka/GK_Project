using System.Collections.Generic;
using UnityEngine;

public class EnemyRangeAttackState : State<Enemy>
{
    private Dictionary<StateType, State<Enemy>> availableStates;
    private ObjectPool<RangedWeapon> rangedWeaponPool;
    private float cooldownTimer = 0f;

    public EnemyRangeAttackState(Enemy controller, StateMachine<Enemy> stateMachine, Dictionary<StateType, State<Enemy>> availableStates, ObjectPool<RangedWeapon> bulletPool)
        : base(controller, stateMachine)
    {
        this.availableStates = availableStates;
        this.rangedWeaponPool = bulletPool;
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
            cooldownTimer = controller.GetWeapon().cooldown;
        }
    }

    private void FireBullet()
    {
        if (controller is IRangedAttacker rangedAttacker)
        {
            RangedWeapon weapon = rangedAttacker.GetRangedWeaponFromPool(controller.transform.position);
           
        }
    }
}
