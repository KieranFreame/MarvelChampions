using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Get Behind Me", menuName = "MarvelChampions/Card Effects/Protection/Events/Get Behind Me")]
public class GetBehindMe : PlayerCardEffect, IOptional
{
    public override void OnDrawn()
    {
        RevealEncounterCardSystem.Instance.EffectCancelers.Add(this);
    }

    public override bool CanResolve()
    {
        return RevealEncounterCardSystem.Instance.CardToReveal.CardType is CardType.Treachery && _owner.ResourcesAvailable(_card) >= _card.CardCost;
    }

    public override async Task Resolve()
    {
        await PlayCardSystem.Instance.InitiatePlayCard(new(_card));
        await ScenarioManager.inst.ActiveVillain.CharStats.InitiateAttack();
        RevealEncounterCardSystem.Instance.EffectCancelers.Remove(this);
    }

    public override void OnDiscard()
    {
        RevealEncounterCardSystem.Instance.EffectCancelers.Remove(this);
    }
}
