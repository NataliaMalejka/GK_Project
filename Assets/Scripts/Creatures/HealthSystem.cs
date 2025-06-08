using System.Collections;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private int maxHelath;
    public int currentHelath;

    private void Awake()
    {
        currentHelath = maxHelath;
    }

    public void GetDmg(int dmg, int duration)
    {
        if(duration > 1)
        {
            ApplyDamageOverTime(dmg, duration);
        }
        else
        {
            ReduceHealth(dmg);
        }          
    }

    private void ReduceHealth(int dmg)
    {
        currentHelath -= dmg;

        if (currentHelath <= 0)
        {
            currentHelath = 0;

            IDamageable damageable = this.gameObject.GetComponent<IDamageable>();

            if (damageable != null)
            {
                damageable.Die();
            }
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
            GetDmg(damagePerSecond, 0);
            yield return new WaitForSeconds(0.5f);
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
}
