using UnityEngine;

public class StamineMixture : MonoBehaviour
{
    [SerializeField] private float stamine;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IPlayer player = collision.GetComponent<IPlayer>();

        if (player != null)
        {
            Player.Instance.staminaSystem.IncreaseStamina(stamine);
            Player.Instance.hudUpdater.updateStaminaBar(Player.Instance.staminaSystem.currentStamina, Player.Instance.staminaSystem.getMaxStamina());

            Destroy(this.gameObject);
        }
    }
}
