using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class LateUpdateManager : MonoBehaviour
{
    private static List<ILateUpdateObserver> observers = new();
    private static List<ILateUpdateObserver> added = new();
    private static List<ILateUpdateObserver> removed = new();

    private void LateUpdate()
    {
        if (added.Count > 0)
        {
            foreach (var observer in added)
            {
                if (!observers.Contains(observer))
                    observers.Add(observer);
            }
            added.Clear();
        }

        if(removed.Count > 0)
        {
            foreach(var observer in removed)
                observers.Remove(observer);

            removed.Clear();
        }

        foreach (var observer in observers)
        {
           observer.ObserveLateUpdate();
        }
        
    }

    public static void AddToList(ILateUpdateObserver observer)
    {
        if (!added.Contains(observer))
            added.Add(observer);
    }

    public static void RemoveFromList(ILateUpdateObserver observer)
    {
        if (!removed.Contains(observer))
            removed.Add(observer);
    }
}
    
