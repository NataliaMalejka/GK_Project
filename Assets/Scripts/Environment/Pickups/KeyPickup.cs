using UnityEngine;

/** 
 * Key pickup object that adds a key to the player when collected.
 * 
 * @author Krzysztof Gach
 * @version 1.0
 */
[RequireComponent(typeof(Collider2D))]
public class KeyPickup : MonoBehaviour, IPickup
{
    public void Collect(GameObject collector)
    {
        KeySystem keySystem = collector.GetComponent<KeySystem>();
        if (keySystem != null)
        {
            keySystem.AddKey();
            // Optional: play sound, animation, etc.
            Destroy(gameObject);
        }
        else
        {
            Debug.LogWarning("Collector does not have a KeySystem component.");
        }
    }
}
