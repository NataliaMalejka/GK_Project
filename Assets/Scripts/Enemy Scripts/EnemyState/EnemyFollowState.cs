using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyFollowState : State<Enemy>
{
    private Dictionary<StateType, State<Enemy>> availableStates;
    private float minDistance = 1;
    private float cooldownTimer = 0f;

    Vector3 direction;

    public EnemyFollowState(Enemy controller, StateMachine<Enemy> stateMachine, Dictionary<StateType, State<Enemy>> availableStates)
        : base(controller, stateMachine)
    {
        this.availableStates = availableStates;
    }

    public override void EnterState()
    {
        cooldownTimer = 0f;
        direction = Player.Instance.transform.position - controller.transform.position;
        controller.GetWeapon().gameObject.SetActive(true);
    }

    public override void UpdateState()
    {

    }

    public override void FixedUpdateState()
    {
        Vector3 playerPos = Player.Instance.transform.position;
        Vector3 enemyPos = controller.transform.position;

        if (playerPos.x < enemyPos.x)
            controller.transform.rotation = Quaternion.Euler(0, -180, 0);
        else
            controller.transform.rotation = Quaternion.Euler(0, 0, 0);

        float distance = Vector2.Distance(enemyPos, playerPos);

        if (!controller.seePlayer)
        {
            stateMachine.ChangeState(availableStates[StateType.Idle]);
            return;
        }

        cooldownTimer -= Time.fixedDeltaTime;

        if (distance > minDistance)
        {
            direction = playerPos - enemyPos;
            Move();
        }
        else
        {
            controller.rb.linearVelocity = Vector2.zero; 
            Attack();
        }
    }

    private void Move()
    {
        controller.rb.linearVelocity = new Vector2(direction.x, direction.y).normalized * controller.speed;
    }

    private void Attack()
    {
        if (cooldownTimer <= 0f)
        {
            if (controller is IMeleeAttack meleeAttacker)
            {
                cooldownTimer = controller.GetWeapon().cooldown;
                meleeAttacker.StartAttack();
            }
        }
    }

    public override void ExitState()
    {
        controller.GetWeapon().gameObject.SetActive(false);
    }
}
