using UnityEngine;

public class ManaSystem : MonoBehaviour
{
    [SerializeField] private int maxMana;
    public int currentmMna;

    private void Awake()
    {
        currentmMna = maxMana;
    }

    public bool CanReduceMana(int amound)
    {
        if (currentmMna >= amound)
            return true;
        else
            return false;
    }

    public void ReudceMana(int amound)
    {
        currentmMna -= amound;
    }    

    public void IncreaseCurrentMana(int amound)
    {
        currentmMna += amound;

        if(currentmMna > maxMana)
            currentmMna = maxMana;
    }

    public void IncreaseMaxMana(int amound)
    {
        maxMana += amound;
        IncreaseCurrentMana(amound);
    }
}
