using UnityEngine;
using UnityEngine.UI;

public class BatteryManager : MonoBehaviour
{
    [SerializeField] public int batteryCount = 0;
    [SerializeField] private Text batteryCountText;

    private void Start()
    {
        UpdateUI();
    }

    public void AddBattery()
    {
        batteryCount++;
        UpdateUI();
        Debug.Log($"Battery collected. Total: {batteryCount}");
    }

    public bool UseBattery(Laser laser)
    {
        if (batteryCount > 0 && laser != null && !laser.IsActive)
        {
            batteryCount--;
            UpdateUI();
            laser.Activate();
            return true;
        }
        return false;
    }

    private void UpdateUI()
    {
        if (batteryCountText != null)
        {
            batteryCountText.text = $"Batteries: {batteryCount}";
        }
    }
}