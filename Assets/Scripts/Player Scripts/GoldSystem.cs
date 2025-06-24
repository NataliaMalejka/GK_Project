using UnityEngine;
using TMPro;

public class GoldSystem : MonoBehaviour
{
    [SerializeField] private int goldAmount = 0;


    public void Awake()
    {

    }

    public int GetGoldAmount() => goldAmount;

    public void SetGoldAmound(int amount)
    {
        goldAmount = amount;
        //UpdateGold();
    }


    public void CollectGold(int amount)
    {
        goldAmount += amount;
        //UpdateGold();
    }

    public bool CanPay(int amount) => goldAmount >= amount;

    public bool ReduceGold(int amount)
    {
        if (CanPay(amount))
        {
            goldAmount -= amount;
            //UpdateGold();
            return true;
        }

        return false;
    }

}
