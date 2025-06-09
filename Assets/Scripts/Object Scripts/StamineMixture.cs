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
            Destroy(this.gameObject);
        }
    }
}
