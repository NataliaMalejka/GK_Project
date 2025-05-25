using UnityEngine;
using UnityEngine.UI;

public class StaminaSystem : MonoBehaviour, IFixedUpdateObserver
{
    [SerializeField] private float staminaIncrease;
    [SerializeField] private float maxStamina;
    public float currentStamina;

    public Image staminaFillBar;

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
        //currentStamina = maxStamina;
    }

    public void ObserveFixedUpdate()
    {
        currentStamina += staminaIncrease;

        if(currentStamina > maxStamina)
        {
            currentStamina = maxStamina;
        }

        staminaFillBar.fillAmount = currentStamina / maxStamina;
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
        staminaFillBar.fillAmount = currentStamina / maxStamina;
    }

    public void IncreaseStamina(float amound)
    {
        currentStamina += amound;
    }

    public void IncreaseMaxStamnina(float amound)
    {
        maxStamina += amound;
        currentStamina += amound;
        staminaFillBar.fillAmount = currentStamina / maxStamina;
    }



    //----------------  //added debug methods:
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            if(CanReduceStamina(20)){ ReduceStamina(20); }  //test: decrease stamina: Num-
        }
    
        if(Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            IncreaseCurrentStamnina(10);  //test: increase stamina: Num+
        }
    }
    public void IncreaseCurrentStamnina(float amound)
    {
        currentStamina += amound;
        staminaFillBar.fillAmount = currentStamina / maxStamina;
    }
}
