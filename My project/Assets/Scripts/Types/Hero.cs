using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Hero
{
    public string Name { get; private set; }
    public Sprite Art { get; private set; }
    public List<string> Traits { get; protected set; } = new List<string>();
    public List<Keywords> Keywords { get; protected set; } = new List<Keywords>();
    public int BaseHandSize { get; protected set; }
    public int HandSize { get; set; }
    public IdentityEffect Effect { get; protected set; }

    public Hero (HeroData data, Player owner)
    {
        //Identity
        Name = data.heroName;
        Art = data.heroArt;
        Traits = data.heroTraits;
        HandSize = BaseHandSize = data.baseHandSize;
        Keywords = data.keywords;

        Effect = data.effect;
        Effect.LoadEffect(owner);
    }
}
