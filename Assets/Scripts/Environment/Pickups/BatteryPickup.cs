using UnityEngine;

/** 
 * Battery pickup object that adds a battery to the player when collected.
 * 
 * @author Krzysztof Gach
 * @version 1.1
 */
[RequireComponent(typeof(Collider2D))]
public class BatteryPickup : MonoBehaviour, IPickup
{
    public void Collect(GameObject collector)
    {
        BatterySystem batterySystem = collector.GetComponent<BatterySystem>();
        if (batterySystem != null)
        {
            batterySystem.AddBattery();
            // Optional: play sound, animation, etc.
            Destroy(gameObject);
        }
        else
        {
            Debug.LogWarning("Collector does not have a BatterySystem component.");
        }
    }
}
