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
    public bool ReachedAllyLimit()
    {
        return Allies.Count > AllyLimit;
    }
}
