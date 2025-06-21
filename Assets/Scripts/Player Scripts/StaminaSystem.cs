using UnityEngine;
using UnityEngine.UI;

public class StaminaSystem : MonoBehaviour, IFixedUpdateObserver
{
    [SerializeField] private float staminaIncrease;
    [SerializeField] private float maxStamina;
    public float currentStamina;

    private Image staminaFillBar; //auto found at runtime


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
        var hudGO = GameObject.FindGameObjectWithTag("HUD");
        if (hudGO == null)
        {
            Debug.LogError("HUD tag missing on your Canvas/HUD!");
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

    public void RegenerateStamina()
    {
        currentStamina = maxStamina;
    }




    private void updateStaminaBar()
    {
        staminaFillBar.fillAmount = currentStamina / maxStamina;
    }
}
