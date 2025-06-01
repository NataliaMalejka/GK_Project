using UnityEngine;

/**
 * Handles player interaction with a laser device.
 * Allows activating a laser using a battery if in range.
 * 
 * @author Krzysztof Gach
 * @version 1.3
 */
public class LaserInteraction : MonoBehaviour
{
    [SerializeField] private Laser laser;
    [SerializeField] private float interactionDistance = 3f;
    [SerializeField] private KeyCode activationKey = KeyCode.E;

    private Transform playerTransform;
    private BatterySystem batterySystem;

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            playerTransform = player.transform;
            batterySystem = player.GetComponent<BatterySystem>();

            if (batterySystem == null)
            {
                Debug.LogWarning("BatterySystem not found on Player!");
            }
        }
        else
        {
            Debug.LogError("Player not found in scene! Make sure it has the 'Player' tag.");
        }
    }

    private void Update()
    {
        if (playerTransform == null || laser == null || batterySystem == null) return;

        float distance = Vector3.Distance(transform.position, playerTransform.position);
        if (distance <= interactionDistance && Input.GetKeyDown(activationKey))
        {
            TryActivateLaser();
        }
    }

    private void TryActivateLaser()
    {
        if (laser.IsPowered)
        {
            Debug.Log("Laser is already powered!");
            return;
        }

        if (batterySystem.UseBattery(laser))
        {
            Debug.Log($"Laser activated! Batteries left: {batterySystem.GetBatteryCount()}");
        }
        else
        {
            Debug.Log("No batteries available!");
        }
    }
}
