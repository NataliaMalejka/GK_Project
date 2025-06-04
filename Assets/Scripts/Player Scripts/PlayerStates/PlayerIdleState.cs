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
        UpdateMovement();
        UpdateAttack();
    }

    private void UpdateMovement()
    {
        controller.xVelocity = Input.GetAxisRaw("Horizontal");
        controller.yVelocity = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.LeftShift) && (controller.xVelocity != 0 || controller.yVelocity != 0))
        {
            if(Player.Instance.staminaSystem.currentStamina >= controller.dashNeededStamina)
            {
                Player.Instance.staminaSystem.ReduceStamina(controller.dashNeededStamina);
                stateMachine.ChangeState(controller.playerDashState);
            }          
        }
    }

    private void UpdateAttack()
    {
        if (Input.GetMouseButtonDown(0) && controller.cooldowntimer <= 0f) 
        {
            controller.weapon = Player.Instance.weaponSwitcher.GetCurrentWeapon();

            if (controller.weapon != null)
            {
                if (Player.Instance.staminaSystem.currentStamina >= controller.weapon.neededStamina && Player.Instance.manaSystem.currentmMna >= controller.weapon.neededMana)

                Player.Instance.staminaSystem.ReduceStamina(controller.weapon.neededStamina);
                Player.Instance.manaSystem.ReudceMana(controller.weapon.neededMana);
                
                controller.weapon.StartAttack();
                controller.cooldowntimer = controller.weapon.cooldown;
            }

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
