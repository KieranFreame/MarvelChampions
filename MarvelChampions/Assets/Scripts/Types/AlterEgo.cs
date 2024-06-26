using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Events;


public class AlterEgo
{
    public string Name { get; private set; }
    public Sprite Art { get; private set; }
    public ObservableSet<string> Traits { get; protected set; } = new();
    public int BaseREC { get; set; }
    public int BaseHP { get; set; }
    public int BaseHandSize { get; protected set; }
    public int HandSize { get; set; }
    public IdentityEffect Effect { get; private set; }
    public event UnityAction SetupComplete;

    public AlterEgo (AlterEgoData data, Player owner)
    {
        //Identity
        BaseHP = data.baseHP;
        HandSize = BaseHandSize = data.baseHandSize;
        Name = data.alterEgoName;
        
        foreach (string trait in data.alterEgoTags)
            Traits.AddItem(trait);

        Art = data.alterEgoArt;

        Effect = data.effect;
        //Effect.LoadEffect(owner);

        //Alter-Ego
        BaseREC = data.baseREC;

        SetupComplete?.Invoke();
    }
}
