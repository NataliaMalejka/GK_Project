using UnityEngine;
using UnityEngine.UI;
using TMPro;

/** 
 *
 * @author Dawid Dulian
 * @version 1.0
 */
//komponent prefabu piesek-astro
public class HudUpdater : MonoBehaviour
{
    [SerializeField] private GameObject lifestarfullPrefab;
    [SerializeField] private GameObject lifestarhalfPrefab;
    [SerializeField] private GameObject lifestarhalffullPrefab;
    [SerializeField] private GameObject lifestaremptyPrefab;

    //auto found at runtime:
    private Image manaFillBar;
    private Image staminaFillBar;
    private TextMeshProUGUI goldAmountText;
    private GameObject healthIconsContainer;


    void Awake()
    {
        // find the canvas root once
        var hudGO = GameObject.FindGameObjectWithTag("HUD");
        if (hudGO == null)
        {
            Debug.LogError("HUD tag missing on your Canvas/HUD!");
            return;
        }



        // find stamina
        var staminaTransform = hudGO.transform.Find("Stamina/Stamina-fill");
        if (staminaTransform != null)
            staminaFillBar = staminaTransform.GetComponent<Image>();
        else
            Debug.LogError("Could not find Stamina/Stamina-fill under HUD!");


        // find mana
        var manaTransform = hudGO.transform.Find("Mana/Mana-fill");
        if (manaTransform != null)
            manaFillBar = manaTransform.GetComponent<Image>();
        else
            Debug.LogError("Could not find Mana/Mana-fill under HUD!");


        // find gold
        var goldTransform = hudGO.transform.Find("Gold");
        if (goldTransform != null)
            goldAmountText = goldTransform.GetComponentInChildren<TextMeshProUGUI>();
        else
            Debug.LogError("Could not find Gold under HUD!");



        // find health
        var healthTransform = hudGO.transform.Find("Health");
        if (healthTransform != null)
            healthIconsContainer = healthTransform.gameObject;
        else
            Debug.LogError("Could not find Health under HUD!");

    }

    public void manualInit(int currentHelath, int getMaxHealth,
            float currentStamina, float getMaxStamina,
            int currentMana, int getMaxMana,
            int GetGoldAmount)
    {
        updateHealthIcons(currentHelath, getMaxHealth);
        updateStaminaBar(currentStamina, getMaxStamina);
        updateManaBar(currentMana, getMaxMana);
        updateGold(GetGoldAmount);
    }

    public void updateManaBar(int currentMana, int maxMana)
    {
        manaFillBar.fillAmount = 1.0f * currentMana / maxMana;
    }


    public void updateStaminaBar(float currentStamina, float maxStamina)
    {
        staminaFillBar.fillAmount = currentStamina / maxStamina;
    }

    public void updateGold(int goldAmount)
    {
        if (goldAmountText != null)
        {
            goldAmountText.text = $"Gold: {goldAmount}";
        }
    }



    public void updateHealthIcons(int currentHelath, int maxHelath)
    {
        if (healthIconsContainer != null)
        {
            foreach (Transform child in healthIconsContainer.transform)
                Destroy(child.gameObject);
        }

        //now update icons for each health unit -> 2 per heart:
        for (int starID = 1; starID <= (maxHelath + 1) / 2; starID++)
        {
            if (lifestarfullPrefab == null || lifestaremptyPrefab == null || lifestarhalfPrefab == null)
            {
                Debug.LogError("HealthSystem: one of your lifestar Prefabs is null! Did you forget to assign it in the Inspector?");
                return;
            }


            //przypisywanie gwiazdek zycia:

            if (currentHelath + 1 == starID * 2) //hp nieparzyste: half
            {
                //np: jestesmy na 3ciej gwiazdce, a mamy 5 hp
                var heart = Instantiate(lifestarhalffullPrefab, Vector3.zero, Quaternion.identity, healthIconsContainer.transform);
            }
            else if (currentHelath >= starID * 2) //hp parzyste: pelne
            {
                //np jestesmy na 4tej lub wczesniejszej gwiazdce, a mamy 8hp -> full
                var heart = Instantiate(lifestarfullPrefab, Vector3.zero, Quaternion.identity, healthIconsContainer.transform);
            }
            //else if ((currentHelath + 1) / 2 > starID)
            //{
            //    //np 50 zycia a jestesmy na 3ciej gwiazdce - mozna ta gwiazdke spokojnie wypelnic pelnym
            //    var heart = Instantiate(lifestarfullPrefab, Vector3.zero, Quaternion.identity, healthIconsContainer.transform);
            //}
            else if ((currentHelath + 1) / 2 < starID)
            {
                //np mam tylko 3 zycia a jestesmy na 40tej gwiazdce - ta i wszystkie dalsze beda puste
                var heart = Instantiate(lifestaremptyPrefab, Vector3.zero, Quaternion.identity, healthIconsContainer.transform);
            }

        }
    }
}
