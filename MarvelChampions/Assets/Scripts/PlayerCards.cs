using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
        set => allyLimit = value;
    }

    public bool ReachedAllyLimit() => Allies.Count > AllyLimit;
    

    public bool HaveResource(Resource resource)
    {
        foreach (var card in Permanents.Where(x => x.Effect is IGenerate))
        {
            IGenerate eff = card.Effect as IGenerate;

            if (eff.CompareResource(resource))
                return true;
        }

        return false;
    }
    
    public int GetResourceCount(Resource resource = Resource.Any)
    {
        int count = 0;

        foreach (var card in Permanents)
        {
            if (card.Effect is IGenerate)
            {
                IGenerate eff = card.Effect as IGenerate;

                if (eff.CompareResource(resource) || resource == Resource.Any)
                    count++;
            }
        }

        return count;
    }
}
