using UnityEngine;
using UnityEngine.UI;

public class GoldSystem : MonoBehaviour, IGameSystem
{
    [SerializeField] private int goldAmount = 0;
    [SerializeField] private Text goldAmountText;

    public void Initialize()
    {
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

    public void UpdateUI()
    {
        if (goldAmountText != null)
        {
            goldAmountText.text = $"Gold: {goldAmount}";
        }
    }
}
