using UnityEngine;

/**
 * Handles player interaction with a laser device.
 * Allows activating a laser using a battery if in range.
 * 
 * @author Krzysztof Gach
 * @version 1.2
 */
public class LaserInteraction : MonoBehaviour
{
    [SerializeField] private Laser laser;
    [SerializeField] private BatteryManager batteryManager;

    [SerializeField] private float interactionDistance = 3f;
    [SerializeField] private KeyCode activationKey = KeyCode.E;

    private Transform playerTransform;

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform ?? Camera.main.transform;
    }

    private void Update()
    {
        if (playerTransform == null || laser == null || batteryManager == null) return;

        float distance = Vector3.Distance(transform.position, playerTransform.position);
        bool inRange = distance <= interactionDistance;

        if (inRange && Input.GetKeyDown(activationKey))
        {
            TryActivateLaser();
        }
    }

    /**
     * Tries to power the laser if not already powered.
     * Consumes a battery if successful.
     */
    private void TryActivateLaser()
    {
        if (laser.IsPowered)
        {
            Debug.Log("Laser is already powered!");
            return;
        }

        if (batteryManager.UseBattery(laser))
        {
            laser.Activate();
            Debug.Log($"Laser activated! Batteries left: {batteryManager.batteryCount}");
        }
        else
        {
            Debug.Log("No batteries available!");
        }
    }
}
