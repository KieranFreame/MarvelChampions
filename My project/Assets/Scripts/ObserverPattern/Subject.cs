using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Subject
{
    public List<Observer> observers = new List<Observer>();
    public dynamic owner;
    public Subject(dynamic owner)
    {
        this.owner = owner;
    }

    public void Attach (Observer observer)
    {
        observers.Add(observer);
    }

    public void Detach(Observer observer)
    {
        observers.Remove(observer);
    }

    public bool Notify()
    {
        foreach(Observer observer in observers)
        {
            observer.Notify();
        }

        return true;
    }
}
