using System.Collections;
using UnityEngine;


//bug: gwiazdki zycia znikaja podczas trafienia w przeciwnika
//needs fix: serializable prefaby trzeba zmienic globalnie, w PlayerController or sth
//todo next: environment/pickups/HealthPickups.cs i ObjectScripts/HealthMixture.cs

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private int maxHelath;
    public int currentHelath;


    public int getMaxHealth() => maxHelath;

    private void Awake()
    {

        currentHelath = maxHelath;
        //updateHealthIcons();
    }

    public void GetDmg(int dmg, int duration)
    {
        if (duration > 1)
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
            //updateHealthIcons();

            IDamageable damageable = this.gameObject.GetComponent<IDamageable>();

            if (damageable != null)
            {
                damageable.Die();
            }
        }
        //updateHealthIcons();
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
            ReduceHealth(damagePerSecond);
            yield return new WaitForSeconds(0.5f);
            elapsed += 1f;
        }
    }

    public void Heal(int amound)
    {
        currentHelath += amound;

        if (currentHelath >= maxHelath)
        {
            currentHelath = maxHelath;
        }
        //updateHealthIcons();
    }

    public void IncreaseMaxHelath(int amound)
    {
        maxHelath += amound;
        Heal(amound);
        //updateHealthIcons();
    }

    public void RegenerateHealth()
    {
        currentHelath = maxHelath;
        //updateHealthIcons();
    }

}
