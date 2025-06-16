using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : State<Enemy>
{
    int nextWaypoint = 1;
    float distToPoint;
    private Dictionary<StateType, State<Enemy>> availableStates;

    public EnemyIdleState(Enemy controller, StateMachine<Enemy> stateMachine, Dictionary<StateType, State<Enemy>> availableStates) : base(controller, stateMachine)
    {
        this.availableStates = availableStates;
    }

    public override void EnterState()
    {
        controller.rb.linearVelocity = Vector3.zero;

        if (controller is IMeleeAttack rangedAttacker)
        {
            int closestIndex = 0;
            float closestDistance = float.MaxValue;

            for (int i = 0; i < controller.wayPoints.Length; i++)
            {
                float dist = Vector2.Distance(controller.transform.position, controller.wayPoints[i].transform.position);
                if (dist < closestDistance)
                {
                    closestDistance = dist;
                    closestIndex = i;
                }
            }

            nextWaypoint = closestIndex;
        }

        if (controller.wayPoints.Length > 1)
        {
            if (!IsFacingWaypoint())
                TakeTurn();
        }

    }

    public override void UpdateState()
    {

    }

    public override void FixedUpdateState()
    {
        if (controller.wayPoints.Length > 1)
            Move();

        if (controller.seePlayer)
        {
            stateMachine.ChangeState(availableStates[StateType.Attack]);
        }
    }

    void Move()
    {
        distToPoint = Vector2.Distance(controller.transform.position, controller.wayPoints[nextWaypoint].transform.position);
        controller.transform.position = Vector2.MoveTowards(controller.transform.position, controller.wayPoints[nextWaypoint].transform.position, controller.speed * Time.fixedDeltaTime);
        if (distToPoint < 0.02f)
        {
            TakeTurn();
        }
    }
    void TakeTurn()
    {
        Vector3 currRot = controller.transform.eulerAngles;
        currRot += controller.wayPoints[nextWaypoint].transform.eulerAngles;
        controller.transform.eulerAngles = currRot;
        chooseNext();
    }

    void chooseNext()
    {
        nextWaypoint++;
        if (nextWaypoint >= controller.wayPoints.Length)
        {
            nextWaypoint = 0;
        }
    }

    bool IsFacingWaypoint()
    {
        Vector2 forward = controller.transform.right;
        Vector2 directionToWaypoint = (controller.wayPoints[nextWaypoint].transform.position - controller.transform.position).normalized;
        float dotProduct = Vector2.Dot(forward, directionToWaypoint);

        return dotProduct > 0;
    }

    public override void ExitState()
    {

    }
}
