using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

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
                Player.Instance.hudUpdater.updateStaminaBar(Player.Instance.staminaSystem.currentStamina, Player.Instance.staminaSystem.getMaxStamina());

                stateMachine.ChangeState(controller.playerDashState);
            }          
        }
    }

    private void UpdateAttack()
    {
        if (Input.GetMouseButtonDown(0) && controller.cooldowntimer <= 0f) 
        {
            controller.weapon = (PlayerWeapon)Player.Instance.weaponSwitcher.GetCurrentWeapon();

            if (controller.weapon != null)
            {
                if (Player.Instance.staminaSystem.currentStamina >= controller.weapon.neededStamina && 
                    Player.Instance.manaSystem.currentMana >= controller.weapon.neededMana)
                {
                    Player.Instance.staminaSystem.ReduceStamina(controller.weapon.neededStamina);
                    Player.Instance.hudUpdater.updateStaminaBar(Player.Instance.staminaSystem.currentStamina, Player.Instance.staminaSystem.getMaxStamina());

                    Player.Instance.manaSystem.ReudceMana(controller.weapon.neededMana);
                    Player.Instance.hudUpdater.updateManaBar(Player.Instance.manaSystem.currentMana, Player.Instance.manaSystem.getMaxMana());


                    controller.StartAttack();
                    controller.cooldowntimer = controller.weapon.cooldown;
                }
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

        HandlePushables();
    }

    private void HandlePushables()
    {
        // Find all pushable objects within radius
        Collider2D[] colliders = Physics2D.OverlapCircleAll(controller.transform.position, 2f);
        List<PushableObject> pushables = new List<PushableObject>();

        foreach (var col in colliders)
        {
            PushableObject pushable = col.GetComponent<PushableObject>();
            if (pushable != null)
            {
                pushables.Add(pushable);
            }
        }

        // Find closest pushable
        PushableObject closestPushable = null;
        float closestDist = Mathf.Infinity;

        foreach (var pushable in pushables)
        {
            Vector2 toPushable = (pushable.transform.position - controller.transform.position);
            float angle = Vector2.Angle(new Vector2(controller.xVelocity, controller.yVelocity), toPushable);

            // SprawdŸ, czy pushable jest mniej wiêcej "przed" graczem (k¹t mniejszy ni¿ 45 stopni)
            if (angle > 45f)
                continue;  // ignoruj obiekty poza osi¹ patrzenia

            float dist = toPushable.magnitude;
            if (dist < closestDist)
            {
                closestDist = dist;
                closestPushable = pushable;
            }
        }

        // Lock all pushables by freezing all constraints
        foreach (var pushable in pushables)
        {
            Rigidbody2D prb = pushable.GetComponent<Rigidbody2D>();
            if (prb != null)
            {
                prb.constraints = RigidbodyConstraints2D.FreezeAll;
                prb.linearVelocity = Vector2.zero; // velocity zamiast linearVelocity
            }
        }

        // Unlock constraints and push the closest pushable
        if (closestPushable != null && new Vector2(controller.xVelocity, controller.yVelocity).sqrMagnitude > 0)
        {
            Rigidbody2D prb = closestPushable.GetComponent<Rigidbody2D>();
            if (prb != null)
            {
                prb.constraints = RigidbodyConstraints2D.FreezeRotation;

                float pushSpeed = controller.runSpeed * 0.5f;  // tweak push speed multiplier
                Vector2 targetVelocity = new Vector2(controller.xVelocity, controller.yVelocity) * pushSpeed;

                prb.linearVelocity = Vector2.Lerp(prb.linearVelocity, targetVelocity, Time.deltaTime * 10f);
            }
        }
    }

    public override void ExitState()
    {
        
    }
}
