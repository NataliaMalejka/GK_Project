using UnityEngine;

/** 
 *
 * @author Krzysztof Gach
 * @version 1.0
 */
[RequireComponent(typeof(Collider2D))]
public class HealthPickup : MonoBehaviour, IPickup
{
    [SerializeField] private int healAmount = 2;

    public void Collect(GameObject collector)
    {
        HealthSystem healthSystem = collector.GetComponent<HealthSystem>();
        if (healthSystem != null)
        {
            healthSystem.Heal(healAmount);
            SoundsManager.Instance.PlayAudioClip(Sounds.CollectCoin);
            Player.Instance.hudUpdater.updateHealthIcons(Player.Instance.healthSystem.currentHelath, Player.Instance.healthSystem.getMaxHealth());
            Destroy(gameObject);
        }
        else
        {
            Debug.LogWarning("Collector does not have a HealthSystem component.");
        }
    }
}
