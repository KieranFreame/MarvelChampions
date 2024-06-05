using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Protective Ward", menuName = "MarvelChampions/Card Effects/Doctor Strange/Protective Ward")]
public class ProtectiveWard : PlayerCardEffect
{
    public override void OnDrawn()
    {
        RevealEncounterCardSystem.Instance.EffectCancelers.Add(this);
    }

    public override bool CanResolve()
    {
        if (RevealEncounterCardSystem.Instance.CardToReveal.CardType != CardType.Treachery) return false;
        if (_owner.Identity.ActiveIdentity is not Hero) return false;
        if (_owner.ResourcesAvailable(_card) < _card.CardCost) return false;

        return true;
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
