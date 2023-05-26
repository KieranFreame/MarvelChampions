using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class PlayerCards
{
    public ObservableCollection<AllyCard> Allies { get; private set; } = new();
    public ObservableCollection<PlayerCard> Permanents { get; private set; } = new();
    public ObservableCollection<CardData> Attachments { get; private set; } = new();
    public int AllyLimit { get; set; } = 3;
    public List<Health> GetHealth()
    {
        List<Health> list = new();
        foreach (AllyCard a in Allies)
           list.Add(a.CharStats.Health);
        
        return list;
    }
    public List<Attacker> GetAttackers()
    {
        List<Attacker> list = new();
        foreach (AllyCard a in Allies)
            list.Add(a.CharStats.Attacker);

        return list;
    }
    public List<Thwarter> GetThwarters()
    {
        List<Thwarter> list = new();
        foreach (AllyCard a in Allies)
            list.Add(a.CharStats.Thwarter);

        return list;
    }

    public bool ReachedAllyLimit()
    {
        return Allies.Count > AllyLimit;
    }
}
