using UnityEngine;

public class BulletShield : MeleeWeapon
{
    public override void StartAttack()
    {
        
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
}
