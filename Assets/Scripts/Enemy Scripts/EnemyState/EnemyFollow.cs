using System.Collections.Generic;
using UnityEngine;

public class EnemyFollow : State<Enemy>
{
    private Dictionary<StateType, State<Enemy>> availableStates;
    private float minDistance = 0.5f;
    private float cooldownTimer = 0f;

    Vector3 direction;

    public EnemyFollow(Enemy controller, StateMachine<Enemy> stateMachine, Dictionary<StateType, State<Enemy>> availableStates)
        : base(controller, stateMachine)
    {
        this.availableStates = availableStates;
    }

    public override void EnterState()
    {
        cooldownTimer = 0f;
        direction = Player.Instance.transform.position - controller.transform.position;
    }

    public override void UpdateState()
    {

    }

    public override void FixedUpdateState()
    {
        if (!controller.seePlayer)
        {
            stateMachine.ChangeState(availableStates[StateType.Idle]);
            return;
        }

        cooldownTimer -= Time.fixedDeltaTime;

        if (Vector2.Distance(controller.transform.position, Player.Instance.transform.position) > minDistance)
            Move();
        else
            Attack();
    }

    private void Move()
    {
        controller.rb.linearVelocity = new Vector2(direction.x, direction.y).normalized * controller.speed;
    }

    private void Attack()
    {
        if(cooldownTimer <= 0f)
        {

        }
    }
}
