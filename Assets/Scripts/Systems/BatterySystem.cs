using UnityEngine;
using UnityEngine.UI;

public class BatterySystem : MonoBehaviour, IGameSystem {
    [SerializeField] private int batteryCount = 0;
    [SerializeField] private Text batteryCountText;

    public void Initialize()
    {
        UpdateUI();
    }

    public void AddBattery()
    {
        batteryCount++;
        UpdateUI();
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

    public int GetBatteryCount() => batteryCount;

    public void UpdateUI()
    {
        if (batteryCountText != null)
        {
            batteryCountText.text = $"Batteries: {batteryCount}";
        }
    }
}
