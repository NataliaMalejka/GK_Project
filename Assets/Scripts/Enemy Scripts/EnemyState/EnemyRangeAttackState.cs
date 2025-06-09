using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

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
            int count = rangedAttacker.GetRangedWeaponCount();

            if (count == 1)
            {
                RangedWeapon weapon = rangedAttacker.GetRangedWeaponFromPool(controller.transform.position);
                weapon.Setcontroller(rangedAttacker.GetController());
            }
            else
            {
                for (int i = 0; i < count; i++)
                {
                    float angle = i * (360f / count);
                    float radians = angle * Mathf.Deg2Rad;
                    Vector3 direction = new Vector3(Mathf.Cos(radians), Mathf.Sin(radians), 0f).normalized;

                    RangedWeapon weapon = rangedAttacker.GetRangedWeaponFromPoolAndSetDirection(controller.transform.position, direction);
                    weapon.Setcontroller(rangedAttacker.GetController());
                        
                }
            }
           
        }
    }
}
