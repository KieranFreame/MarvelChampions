using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Main Effect: After the villain attacks you, discard this and take 2 indirect damage. Spend 2 physical resources to remove this
/// </summary>

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

    #region IndirectDamage
    private void IsTriggerMet(AttackAction action)
    {
        if (action.Owner != (ICharacter)_owner)
            return;

        EffectManager.Inst.Resolving.Push(this);
    }

    public override async Task Resolve()
    {
        List<ICharacter> targets = new()
        {
            TurnManager.instance.CurrPlayer
        };

        targets.AddRange(TurnManager.instance.CurrPlayer.CardsInPlay.Allies);

        await IndirectDamageHandler.inst.HandleIndirectDamage(targets, 2);

        Detach();
        ScenarioManager.inst.EncounterDeck.Discard(Card);
    }
    #endregion

    #region RemoveCard
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
    #endregion

    public override void Attach()
    {
        attached.Attachments.Add(Card as IAttachment);
        AttackSystem.Instance.OnAttackCompleted.Add(IsTriggerMet);
    }

    
    public override void Detach()
    {
        attached.Attachments.Remove(Card as IAttachment);
        AttackSystem.Instance.OnAttackCompleted.Remove(IsTriggerMet);
    }
}
