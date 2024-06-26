using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Vibranium Armor", menuName = "MarvelChampions/Card Effects/Under Attack/Vibranium Armor")]
public class VibraniumArmor : AttachmentCardEffect
{
    public override Task WhenRevealed(Villain owner, EncounterCard card, Player player)
    {
        attached = _owner = owner;
        Card = card;

        Attach();

        return Task.CompletedTask;
    }

    private void OnTakeDamage(DamageAction action)
    {
        if (action.Value > 0) { attached.CharStats.Health.Tough = true; }
    }

    public override bool CanActivate(Player p)
    {
        if (p.Exhausted)
            return false;

        if (!p.HaveResource(Resource.Physical))
            return false;

        return true;
    }

    public override async Task Activate(Player p)
    {
        await PayCostSystem.instance.GetResources(new() { { Resource.Physical, 2 } });
        p.Exhaust();

        Detach();
        ScenarioManager.inst.EncounterDeck.Discard(Card);
    }

    public override void Attach()
    {
        attached.Attachments.Add(Card as IAttachment);
        attached.CharStats.Health.OnTakeDamage += OnTakeDamage;
    }

    public override void Detach()
    {
        attached.Attachments.Remove(Card as IAttachment);
        attached.CharStats.Health.OnTakeDamage -= OnTakeDamage;
    }
}
