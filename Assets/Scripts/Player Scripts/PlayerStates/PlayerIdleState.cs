using UnityEngine;

public class PlayerIdleState : State<PlayerController>
{
    public PlayerIdleState(PlayerController controller, StateMachine<PlayerController> stateMachine) : base(controller, stateMachine)
    {
    }

    public override void EnterState()
    {
        
    }

    public override void UpdateState()
    {     
        controller.xVelocity = Input.GetAxisRaw("Horizontal");
        controller.zVelocity = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.LeftShift) && (controller.xVelocity !=0 || controller.zVelocity !=0))
        {           
            stateMachine.ChangeState(controller.playerDashState);
        }
    }

    public override void FixedUpdateState()
    {
        move();
    }

    public override void LateUpdateState()
    {
        if (controller.xVelocity != 0)
            controller.direction = controller.xVelocity;
    }

    private void move()
    {
        Vector3 inputDirection = new Vector3(controller.xVelocity, 0, controller.zVelocity);

        if (inputDirection.magnitude > 1f)
        {
            inputDirection = inputDirection.normalized;
        }

        controller.rb.linearVelocity = inputDirection * controller.runSpeed;
    }

    public override void ExitState()
    {
        
    }
}
