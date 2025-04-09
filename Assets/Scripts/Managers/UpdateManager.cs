using System;
using System.Collections.Generic;
using UnityEngine;

public class UpdateManager : MonoBehaviour
{
    private static List<IUpdateObserver> observers = new();
    private static List<IUpdateObserver> added = new();
    private static List<IUpdateObserver> removed = new();

    private void Update()
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

        if (removed.Count > 0)
        {
            foreach (var observer in removed)
                observers.Remove(observer);

            removed.Clear();
        }

        foreach (var observer in observers)
        {
            observer.ObserveUpdate();
        }
    }

    public static void AddToList(IUpdateObserver observer)
    {
        if(!added.Contains(observer))
            added.Add(observer);
    }

    public static void RemoveFromList(IUpdateObserver observer)
    {
        if(!removed.Contains(observer))
            removed.Add(observer);
    }
}
