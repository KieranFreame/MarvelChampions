using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Ball and Chain", menuName = "MarvelChampions/Card Effects/RotRS/Absorbing Man/Ball and Chain")]
public class BallAndChain : AttachmentCardEffect
{
    public override Task WhenRevealed(Villain owner, EncounterCard card, Player player)
    {
        _owner = owner;
        Card = card;

        Attach();

        return Task.CompletedTask;
    }

    public override bool CanActivate(Player player)
    {
        return player.HaveResource(Resource.Physical);
    }

    public override async Task Activate(Player player)
    {
        await PayCostSystem.instance.GetResources(new() { { Resource.Physical, 1 } });

        EncounterCardData data = _card.Data;

        Detach();
        ScenarioManager.inst.EncounterDeck.Discard(Card);
        ScenarioManager.inst.EncounterDeck.discardPile.Remove(data);
        ScenarioManager.inst.EncounterDeck.AddToDeck(data);
    }

    public override void Attach()
    {
        _owner.CharStats.Attacker.CurrentAttack++;
        _owner.CharStats.Schemer.CurrentScheme++;
    }

    public override void Detach()
    {
        _owner.CharStats.Attacker.CurrentAttack--;
        _owner.CharStats.Schemer.CurrentScheme--;
    }

    public override async Task Boost(Action action)
    {
        var card = GameObject.Find("EncounterCards").transform.Find("Ball and Chain").GetComponent<EncounterCard>();
        await RevealEncounterCardSystem.Instance.InitiateRevealCard(card);
    }
}
