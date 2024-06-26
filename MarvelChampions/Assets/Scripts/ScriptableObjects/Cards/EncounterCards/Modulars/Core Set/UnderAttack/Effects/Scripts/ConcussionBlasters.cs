using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Concussion Blasters", menuName = "MarvelChampions/Card Effects/Under Attack/Concussion Blasters")]
public class ConcussionBlasters : AttachmentCardEffect
{
    Retaliate _retaliate;
    public override Task WhenRevealed(Villain owner, EncounterCard card, Player player)
    {
        attached = _owner = owner;

        Attach();

        return Task.CompletedTask;
    }

    public override bool CanActivate(Player p)
    {
        if (p.Exhausted)
            return false;

        if (!p.HaveResource(Resource.Energy))
            return false;

        return true;
    }

    public override async Task Activate(Player p)
    {
        await PayCostSystem.instance.GetResources(new() { { Resource.Energy, 2 } });
        p.Exhaust();

        Detach();
        ScenarioManager.inst.EncounterDeck.Discard(Card);
    }

    public override void Attach()
    {
        attached.Attachments.Add(Card as IAttachment);
        _retaliate = new(attached, 1);
    }

    public override void Detach()
    {
        _retaliate.WhenRemoved();
        attached.Attachments.Remove(Card as IAttachment);
    }
}
