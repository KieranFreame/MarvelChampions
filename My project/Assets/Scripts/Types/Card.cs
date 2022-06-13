using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Card
{
    public CardData data;
    public List<Action> actions = new List<Action>();
    public AbilityLoader abilityLoader;
    public bool inPlay;
    public bool exhausted;

    public Card (CardData data)
    {
        this.data = data;
        abilityLoader = LoaderFactory.instance.CreateLoader(this);

        foreach (ActionData action in data.actions)
        {
            var _action = ActionFactory.instance.CreateAction(action);
            abilityLoader.AddAction(_action);
        }
    }

    public List<Resource> GetResource(PlayerCard card)
    {
        return (data as PlayerCard).GetResource(card);
    }
}
