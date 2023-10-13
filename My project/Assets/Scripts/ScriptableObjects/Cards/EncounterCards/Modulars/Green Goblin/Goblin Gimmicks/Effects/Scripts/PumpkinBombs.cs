using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Pumpkin Bombs", menuName = "MarvelChampions/Card Effects/Modulars/Goblin Gimmicks/Pumpkin Bombs")]
public class PumpkinBombs : AttachmentCardEffect
{
    public override Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
    {
        attached = _owner = owner;
        Card = card;

        Attach();

        return Task.CompletedTask;
    }

    private void AttackInitiated() => AttackSystem.Instance.OnAttackCompleted.Add(AttackCompleted);

    private async Task AttackCompleted(AttackAction action)
    {
        AttackSystem.Instance.OnAttackCompleted.Remove(AttackCompleted);
        Detach();

        List<ICharacter> targets = new()
        {
            (action.Target is Player) ? action.Target : (action.Target as AllyCard).Owner
        };

        targets.AddRange((targets[0] as Player).CardsInPlay.Allies.Where(x => x != action.Target as UnityEngine.Object));

        await IndirectDamageHandler.inst.HandleIndirectDamage(targets, 2);

        ScenarioManager.inst.EncounterDeck.Discard(Card);
    }

    public override bool CanActivate(Player player)
    {
        return player.HaveResource(Resource.Physical, 2);
    }

    public override async Task Activate(Player player)
    {
        await PayCostSystem.instance.GetResources(Resource.Physical, 2);

        Detach();
        ScenarioManager.inst.EncounterDeck.Discard(Card);
    }

    public override void Attach()
    {
        attached.Attachments.Add(Card as IAttachment);
        attached.CharStats.AttackInitiated += AttackInitiated;
    }

    public override void Detach()
    {
        attached.Attachments.Remove(Card as IAttachment);
        attached.CharStats.AttackInitiated -= AttackInitiated;
    }
}
