using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class AlterEgo
{
    public string Name { get; private set; }
    public Sprite Art { get; private set; }
    public List<string> Traits { get; protected set; } = new List<string>();
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
        Traits = data.alterEgoTags;
        Art = data.alterEgoArt;

        Effect = data.effect;
        Effect.LoadEffect(owner);

        //Alter-Ego
        BaseREC = data.baseREC;

        SetupComplete?.Invoke();
    }
}
