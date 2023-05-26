using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Subject
{
    private List<Observer> Observers = new List<Observer>();

    public void Subscribe(Observer subscriber)
    {
        Observers.Add(subscriber);
    }

    public void Unsubscribe(Observer subscriber)
    {
        if (Observers.Contains(subscriber))
            Observers.Remove(subscriber);
    }

    public void Invoke()
    {
        foreach (Observer o in Observers)
        {
            o.Notify();
        }
    }
    
    public void Invoke(int value)
    {
        foreach (Observer o in Observers)
        {
            o.Notify(value);
        }
    }
}
