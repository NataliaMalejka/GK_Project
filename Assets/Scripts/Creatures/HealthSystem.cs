using System.Collections;
using UnityEngine;


//bug: gwiazdki zycia znikaja podczas trafienia w przeciwnika
//needs fix: serializable prefaby trzeba zmienic globalnie, w PlayerController or sth

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private int maxHelath;
    public int currentHelath;

    [SerializeField] private GameObject lifestarfullPrefab;
    [SerializeField] private GameObject lifestarhalfPrefab;
    [SerializeField] private GameObject lifestarhalffullPrefab;
    [SerializeField] private GameObject lifestaremptyPrefab;

    private GameObject healthIconsContainer; //auto found at runtime


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
        var healthTransform = hudGO.transform.Find("Health");
        if (healthTransform == null)
            Debug.LogError("Could not find Health under HUD!");

        healthIconsContainer = healthTransform.gameObject;


        currentHelath = maxHelath;
        updateHealthIcons();
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
            updateHealthIcons();

            IDamageable damageable = this.gameObject.GetComponent<IDamageable>();

            if (damageable != null)
            {
                damageable.Die();
            }
        }
        updateHealthIcons();
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
        updateHealthIcons();
    }

    public void IncreaseMaxHelath(int amound)
    {
        maxHelath += amound;
        Heal(amound);
        updateHealthIcons();
    }

    public void RegenerateHealth()
    {
        currentHelath = maxHelath;
    }

    private void updateHealthIcons()
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
