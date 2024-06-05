using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Enhanced Spider-Sense", menuName = "MarvelChampions/Card Effects/Spider-Man (Peter Parker)/Enhanced Spider-Sense")]
public class EnhancedSpiderSense : PlayerCardEffect, IOptional
{
    public override void OnDrawn()
    {
        RevealEncounterCardSystem.Instance.EffectCancelers.Add(this);
    }

    public override bool CanResolve()
    {
        return RevealEncounterCardSystem.Instance.CardToReveal.CardType == CardType.Treachery && _owner.ResourcesAvailable(_card) >= _card.CardCost;    
    }

    public override async Task Resolve()
    {
        await PlayCardSystem.Instance.InitiatePlayCard(new(_card));
    }

    public override void OnDiscard()
    {
        RevealEncounterCardSystem.Instance.EffectCancelers.Remove(this);
    }

}
