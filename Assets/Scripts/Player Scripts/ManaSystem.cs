using UnityEngine;
using UnityEngine.UI;

public class ManaSystem : MonoBehaviour
{
    [SerializeField] private int maxMana;
    public int currentMana;

    [Header("UI (auto-found at runtime)")]
    [SerializeField] private Image manaFillBar;

    private void Awake()
    {
        // find the canvas root once
        var hudGO = GameObject.FindGameObjectWithTag("HUD_Canvas");
        if (hudGO == null)
        {
            Debug.LogError("HUD_Canvas tag missing on your HUD!");
            return;
        }

        // now find your specific child by name:
        var staminaTransform = hudGO.transform.Find("Mana/Mana-fill");
        if (staminaTransform != null)
            manaFillBar = staminaTransform.GetComponent<Image>();
        else
            Debug.LogError("Could not find Mana/Mana-fill under HUD!");


        currentMana = maxMana;
        updateManaBar();
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
        updateManaBar();
    }    

    public void IncreaseCurrentMana(int amound)
    {
        currentMana += amound;

        if(currentMana > maxMana)
            currentMana = maxMana;

        updateManaBar();
    }

    public void IncreaseMaxMana(int amound)
    {
        maxMana += amound;
        IncreaseCurrentMana(amound);

        updateManaBar();
    }


    private void updateManaBar()
    {
        manaFillBar.fillAmount = currentMana / maxMana;
    }
}
