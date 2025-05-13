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
        controller.yVelocity = Input.GetAxisRaw("Vertical");

        Debug.Log(controller.xVelocity);
        Debug.Log(controller.yVelocity);

        if (Input.GetKeyDown(KeyCode.LeftShift) && (controller.xVelocity !=0 || controller.yVelocity !=0))
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
        Debug.Log("move");
        Vector2 inputDirection = new Vector2(controller.xVelocity, controller.yVelocity);

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
