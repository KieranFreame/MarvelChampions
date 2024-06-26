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
        owner.VillainTraits.AddItem("Ice");
        owner.VillainTraits.AddItem("Metal");
        owner.VillainTraits.AddItem("Stone");
        owner.VillainTraits.AddItem("Wood");
        return Task.CompletedTask;
    }

    public override Task WhenDefeated()
    {
        _owner.VillainTraits.RemoveItem("Ice");
        _owner.VillainTraits.RemoveItem("Metal");
        _owner.VillainTraits.RemoveItem("Wood");
        _owner.VillainTraits.RemoveItem("Stone");

        _owner.VillainTraits.AddItem(NoneShallPass.CurrentEnvironment.CardTraits.Collection[0]);

        return Task.CompletedTask;
    }

    public override async Task Boost(Action action)
    {
        var card = GameObject.Find("EncounterCards").transform.Find("Super Absorbing Power").GetComponent<EncounterCard>();
        await RevealEncounterCardSystem.Instance.InitiateRevealCard(card);
    }
}
