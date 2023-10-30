using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class PlayerCards
{
    public ObservableCollection<AllyCard> Allies { get; private set; } = new();
    public ObservableCollection<PlayerCard> Permanents { get; private set; } = new();
    public ObservableCollection<IAttachment> Attachments { get; private set; } = new();

    private int allyLimit = 3;

    public int AllyLimit
    {
        get => allyLimit;
        set
        {
            allyLimit = value;
            Debug.Log("Ally Limit: " + allyLimit);
        }
    }

    public bool ReachedAllyLimit()
    {
        return Allies.Count > AllyLimit;
    }

    public bool HaveResource(Resource resource)
    {
        foreach (var card in Permanents)
        {
            if (card.Effect is IGenerate)
            {
                IGenerate eff = card.Effect as IGenerate;

                if (eff.CompareResource(resource))
                    return true;
            }
        }

        return false;
    }
    
    public int HaveResource(Resource resource, int amount = 1)
    {
        int count = 0;

        foreach (var card in Permanents)
        {
            if (card.Effect is IGenerate)
            {
                IGenerate eff = card.Effect as IGenerate;

                if (eff.CompareResource(resource))
                    count++;
            }
        }

        return count;
    }
}
