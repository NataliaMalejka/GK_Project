using System.Collections.Generic;
using UnityEngine;

public class FixedUpdateManager : MonoBehaviour
{
    private static List<IFixedUpdateObserver> observers = new();
    private static List<IFixedUpdateObserver> added = new();
    private static List<IFixedUpdateObserver> removed = new();

    private void FixedUpdate()
    {
        if(added.Count > 0)
        {
            foreach(var observer in added)
            {
                if(!observers.Contains(observer))
                    observers.Add(observer);
            }
            added.Clear();
        }

        if(removed.Count > 0)
        {
            foreach (var observer in removed)
                observers.Remove(observer);

            removed.Clear();
        }

        foreach(var observer in observers)
        {
            observer.ObserveFixedUpdate();
        }


        //Player.Instance.hudUpdater.updateHealthIcons(Player.Instance.healthSystem.currentHelath, Player.Instance.healthSystem.getMaxHealth());
        Player.Instance.hudUpdater.updateStaminaBar(Player.Instance.staminaSystem.currentStamina, Player.Instance.staminaSystem.getMaxStamina());
        //Player.Instance.hudUpdater.updateManaBar(Player.Instance.manaSystem.currentMana, Player.Instance.manaSystem.getMaxMana());
    }

    public static void AddToList(IFixedUpdateObserver observer)
    {
        if(!added.Contains(observer)) 
            added.Add(observer);
    }

    public static void RemoveFromList(IFixedUpdateObserver observer)
    {
        if(!removed.Contains(observer))
            removed.Add(observer);
    }
}
