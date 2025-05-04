using UnityEngine;

public class GoldSystem
{
    private int goldAmound = 0;

    public bool CanPay(int amound)
    {
        if(goldAmound >= amound)
            return true; 
        else
            return false;
    }

    public void ReduceGold(int amound)
    {
        goldAmound -= amound;
    }

    public void CollectGold(int amound)
    {
        goldAmound += amound;
    }
}
