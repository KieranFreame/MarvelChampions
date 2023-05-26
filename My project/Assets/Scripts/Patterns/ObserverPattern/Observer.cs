using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Observer
{
    public dynamic owner;

    public delegate void onNotify();
    public delegate void onNotifyInt(int value);

    public onNotify notify;
    public onNotifyInt notifyInt;

    public Observer(dynamic owner)
    {
        this.owner = owner;
    }

    public void Notify()
    {
        notify?.Invoke();
    }
    
    public void Notify(int value)
    {
        notifyInt?.Invoke(value);
    }
}
