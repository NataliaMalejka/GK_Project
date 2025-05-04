using UnityEngine;

public class PlayerDashState : State<PlayerController>
{
    private int currentDashCount;

    public PlayerDashState(PlayerController controller, StateMachine<PlayerController> stateMachine) : base(controller, stateMachine)
    {
    }

    public override void EnterState()
    {
        currentDashCount = controller.dashTime;
        controller.rb.linearVelocity = Vector3.zero;
    }

    public override void UpdateState()
    {

    }

    public override void FixedUpdateState()
    {
        if (currentDashCount > 0)
        {
            move();
            currentDashCount--;
        }
        else
        {
            stateMachine.ChangeState(controller.playerIdleState);
        }
    }

    private void move()
    {
        Vector3 inputDirection = new Vector3(controller.xVelocity, controller.yVelocity, 0);

        if (inputDirection.magnitude > 1f)
        {
            inputDirection = inputDirection.normalized;
        }

        controller.rb.linearVelocity = inputDirection * controller.dashForce;
    }

    public override void LateUpdateState()
    {
        
    }

    public override void ExitState()
    {

    }
}
