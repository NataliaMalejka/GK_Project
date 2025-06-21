using UnityEngine;
using TMPro;

public class GoldSystem : MonoBehaviour
{
    [SerializeField] private int goldAmount = 0;

    private TextMeshProUGUI goldAmountText; //auto found at runtime


    public void Awake()
    {
        Initialize();
    }
    public void Initialize()
    {
        // find the canvas root once
        var hudGO = GameObject.FindGameObjectWithTag("HUD");
        if (hudGO == null)
        {
            Debug.LogError("HUD tag missing on your Canvas/HUD!");
            return;
        }

        // now find your specific child by name:
        var goldTransform = hudGO.transform.Find("Gold");
        if (goldTransform != null)
            goldAmountText = goldTransform.GetComponentInChildren<TextMeshProUGUI>();
        else
            Debug.LogError("Could not find Gold under HUD!");

        UpdateUI();
    }

    public void CollectGold(int amount)
    {
        goldAmount += amount;
        UpdateUI();
    }

    public bool CanPay(int amount) => goldAmount >= amount;

    public bool ReduceGold(int amount)
    {
        if (CanPay(amount))
        {
            goldAmount -= amount;
            UpdateUI();
            return true;
        }

        return false;
    }

    public int GetGoldAmount() => goldAmount;

    public void SetGoldAmound(int amount)
    {
        goldAmount = amount;
    }

    public void UpdateUI()
    {
        if (goldAmountText != null)
        {
            goldAmountText.text = $"Gold: {goldAmount}";
        }
    }
}
