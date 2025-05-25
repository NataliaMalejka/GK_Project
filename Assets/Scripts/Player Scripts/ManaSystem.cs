using UnityEngine;
using UnityEngine.UI;

public class ManaSystem : MonoBehaviour
{
    [SerializeField] private int maxMana;
    public int currentmMna;

    public Image manaFillBar;

    private void Awake()
    {
        //currentmMna = maxMana;
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

        manaFillBar.fillAmount = (float)currentmMna / maxMana;
    }    

    public void IncreaseCurrentMana(int amound)
    {
        currentmMna += amound;

        if(currentmMna > maxMana)
            currentmMna = maxMana;

        manaFillBar.fillAmount = (float)currentmMna / maxMana;
    }

    public void IncreaseMaxMana(int amound)
    {
        maxMana += amound;
        IncreaseCurrentMana(amound);
        manaFillBar.fillAmount = (float)currentmMna / maxMana;
    }


    //----------------  //added debug methods
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (CanReduceMana(20)) { ReudceMana(20); }  //test: decrease mana: 'f'
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            IncreaseCurrentMana(10);  //test: increase mana: 'm'
        }
    }

}
