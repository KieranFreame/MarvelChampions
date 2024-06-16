using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Super Absorbing Power", menuName = "MarvelChampions/Card Effects/RotRS/Absorbing Man/Super Absorbing Power")]
public class SuperAbsorbingPower : EncounterCardEffect
{
    public override Task WhenRevealed(Villain owner, EncounterCard card, Player player)
    {
        _owner = owner;
        (card as SchemeCard).Threat.CurrentThreat *= TurnManager.Players.Count;
        owner.VillainTraits.Add("Ice");
        owner.VillainTraits.Add("Metal");
        owner.VillainTraits.Add("Stone");
        owner.VillainTraits.Add("Wood");
        return Task.CompletedTask;
    }

    public override Task WhenDefeated()
    {
        _owner.VillainTraits.Remove("Ice");
        _owner.VillainTraits.Remove("Metal");
        _owner.VillainTraits.Remove("Wood");
        _owner.VillainTraits.Remove("Stone");

        return Task.CompletedTask;
    }

    public override async Task Boost(Action action)
    {
        var card = GameObject.Find("EncounterCards").transform.Find("Super Absorbing Power").GetComponent<EncounterCard>();
        await RevealEncounterCardSystem.Instance.InitiateRevealCard(card);
    }
}
