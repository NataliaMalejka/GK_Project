using UnityEngine;
using UnityEngine.UI;

public class StaminaSystem : MonoBehaviour, IFixedUpdateObserver
{
    [SerializeField] private float staminaIncrease;
    [SerializeField] private float maxStamina;
    public float currentStamina;

    [Header("UI (auto-found at runtime)")]
    [SerializeField] private Image staminaFillBar;


    private void OnEnable()
    {
        FixedUpdateManager.AddToList(this);
    }

    private void OnDisable()
    {
        FixedUpdateManager.RemoveFromList(this);
    }

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
        var staminaTransform = hudGO.transform.Find("Stamina/Stamina-fill");
        if (staminaTransform != null)
            staminaFillBar = staminaTransform.GetComponent<Image>();
        else
            Debug.LogError("Could not find Stamina/Stamina-fill under HUD!");


        currentStamina = maxStamina;
        updateStaminaBar();
    }

    public void ObserveFixedUpdate()
    {
        currentStamina += staminaIncrease;

        if(currentStamina > maxStamina)
        {
            currentStamina = maxStamina;
        }
        updateStaminaBar();
    }

    public bool CanReduceStamina(float amound)
    {
        if (currentStamina >= amound)
            return true;
        else
            return false;
    }

    public void ReduceStamina(float amound)
    {
        currentStamina -= amound;
        updateStaminaBar();
    }

    public void IncreaseStamina(float amound)
    {
        currentStamina += amound;
        updateStaminaBar();
    }

    public void IncreaseMaxStamnina(float amound)
    {
        maxStamina += amound;
        currentStamina += amound;
        updateStaminaBar();
    }


    private void updateStaminaBar()
    {
        staminaFillBar.fillAmount = currentStamina / maxStamina;
    }
}
