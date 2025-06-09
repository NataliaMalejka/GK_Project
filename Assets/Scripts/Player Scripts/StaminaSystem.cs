using UnityEngine;

public class StaminaSystem : MonoBehaviour, IFixedUpdateObserver
{
    [SerializeField] private float staminaIncrease;
    [SerializeField] private float maxStamina;
    public float currentStamina;

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
    }

    public void ObserveFixedUpdate()
    {
        currentStamina += staminaIncrease;

        if(currentStamina > maxStamina)
        {
            currentStamina = maxStamina;
        }
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
    }

    public void IncreaseStamina(float amound)
    {
        currentStamina += amound;
    }

    public void IncreaseMaxStamnina(float amound)
    {
        maxStamina += amound;
        currentStamina += amound;
    }
}
