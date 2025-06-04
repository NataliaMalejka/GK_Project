using System.Collections;
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

    public void ApplyDamageOverTime(int damagePerSecond, int duration)
    {
        StartCoroutine(DamageOverTimeCoroutine(damagePerSecond, duration));
    }

    private IEnumerator DamageOverTimeCoroutine(int damagePerSecond, int duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            GetDmg(damagePerSecond);
            yield return new WaitForSeconds(1f);
            elapsed += 1f;
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
