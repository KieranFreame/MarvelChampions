using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Hero
{
    public string Name { get; protected set; }
    public List<string> Traits { get; protected set; } = new List<string>();
    public List<Keywords> Keywords { get; protected set; } = new List<Keywords>();
    public int BaseHandSize { get; protected set; }
    public IdentityEffect Effect { get; protected set; }

    public Hero (HeroData data, Player owner)
    {
        //Identity
        Name = data.heroName;
        Traits = data.heroTraits;
        BaseHandSize = data.baseHandSize;
        Keywords = data.keywords;

        Effect = data.effect;
        Effect.LoadEffect(owner);
    }
}