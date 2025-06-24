using UnityEngine;
using UnityEngine.UI;


public class ManaSystem : MonoBehaviour
{
    [SerializeField] private int maxMana;
    public int currentMana;


    public int getMaxMana() => maxMana;

    private void Awake()
    {
        currentMana = maxMana;
        //updateManaBar();
    }

    public bool CanReduceMana(int amound)
    {
        if (currentMana >= amound)
            return true;
        else
            return false;
    }

    public void ReudceMana(int amound)
    {
        currentMana -= amound;
        //updateManaBar();
    }

    public void IncreaseCurrentMana(int amound)
    {
        currentMana += amound;

        if (currentMana > maxMana)
            currentMana = maxMana;
        //updateManaBar();
    }

    public void IncreaseMaxMana(int amound)
    {
        maxMana += amound;
        IncreaseCurrentMana(amound);

        //updateManaBar();
    }

    public void RegenerateMana()
    {
        currentMana = maxMana;

        //updateManaBar();
    }

}
