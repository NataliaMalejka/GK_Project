using UnityEngine;

public class HealthMixture : MonoBehaviour
{
    [SerializeField] private int health;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IPlayer player = collision.GetComponent<IPlayer>();

        if (player != null)
        {
            Player.Instance.healthSystem.Heal(health);
            SoundsManager.Instance.PlayAudioClip(Sounds.CollectMixtures);
            Player.Instance.hudUpdater.updateHealthIcons(Player.Instance.healthSystem.currentHelath, Player.Instance.healthSystem.getMaxHealth());
            Destroy(this.gameObject);
        }
    }
}
