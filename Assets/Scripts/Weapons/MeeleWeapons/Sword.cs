using UnityEngine;

public class Sword : MeeleWeapon
{
    private Collider weaponCollider;
    private Animator animator;

    protected override void Awake()
    {
        weaponCollider = GetComponent<Collider>();
        animator = GetComponent<Animator>();
    }

    public void StartAttack()
    {
        animator.SetTrigger("Attack");
    }

    private void OnTriggerEnter(Collider other)
    {
        IPlayer player = other.GetComponent<IPlayer>();

        if (player != null)
        {
            if (Player.Instance.controller.stateMachine.currentState != Player.Instance.controller.playerDashState)
            {
                Player.Instance.healthSystem.GetDmg(dmg);
            }
        }
    }

    public void EnableCollider()
    {
        weaponCollider.enabled = true;
    }

    public void DisenableCollider()
    {
        weaponCollider.enabled = false;
    }
}
