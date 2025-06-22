using UnityEngine;
using UnityEngine.UI;

public class StaminaSystem : MonoBehaviour, IFixedUpdateObserver
{
    [SerializeField] private float staminaIncrease;
    [SerializeField] private float maxStamina;
    public float currentStamina;


    public float getMaxStamina() => maxStamina;


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
        currentStamina = maxStamina;
        //updateStaminaBar();
    }

    public void ObserveFixedUpdate()
    {
        currentStamina += staminaIncrease;

        if(currentStamina > maxStamina)
        {
            currentStamina = maxStamina;
        }
        //updateStaminaBar(); // - done in FixedUpdateManager.cs
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
        //updateStaminaBar();
    }

    public void IncreaseStamina(float amound)
    {
        currentStamina += amound;
        //updateStaminaBar();
    }

    public void IncreaseMaxStamnina(float amound)
    {
        maxStamina += amound;
        currentStamina += amound;
        //updateStaminaBar();
    }

    public void RegenerateStamina()
    {
        currentStamina = maxStamina;
        //updateStaminaBar();
    }


}
