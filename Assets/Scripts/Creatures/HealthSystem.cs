using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private int maxHelath;
    private int currentHelath;

    private void Awake()
    {
        currentHelath = maxHelath;
    }

    public void GetDmg(int dmg)
    {
        currentHelath -= dmg;

        if(currentHelath <= 0)
        {
            currentHelath = 0;
            Die();
        }               
    }

    public void Heal(int amound)
    {
        currentHelath += amound;

        if(currentHelath >= maxHelath)
        {
            currentHelath = maxHelath;
        }
    }

    public void IncreaseMaxHelath(int amound)
    {
        maxHelath += amound;
        Heal(amound);
    }

    private void Die()
    {

    }
}
