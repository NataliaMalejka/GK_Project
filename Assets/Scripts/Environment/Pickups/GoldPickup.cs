using UnityEngine;

/** 
 * GoldPickup is a component that can be attached to GameObjects in Unity to represent a collectible gold item.
 *
 * @author Krzysztof Gach
 * @version 1.0
 */
[RequireComponent(typeof(Collider2D))]
public class GoldPickup : MonoBehaviour, IPickup
{
    [SerializeField] private int goldValue = 1;

    public void Collect(GameObject collector)
    {
        GoldSystem goldSystem = collector.GetComponent<GoldSystem>();
        if (goldSystem != null)
        {
            goldSystem.CollectGold(goldValue);
            // Optional: play sound, animation, etc.
            Destroy(gameObject);
        }
        else
        {
            Debug.LogWarning("Collector does not have a GoldSystem component.");
        }
    }
}