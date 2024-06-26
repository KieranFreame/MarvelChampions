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
    public override Task OnEnterPlay()
    {
        attached = _owner;

        Attach();

        return Task.CompletedTask;
    }

    #region IndirectDamage
    private void IsTriggerMet(Action action)
    {
        if (action is not AttackAction || action.Owner != Owner)
            return;

        EffectManager.Inst.Resolving.Push(this);
    }

    public override async Task Resolve()
    {
        List<ICharacter> targets = new(TurnManager.instance.CurrPlayer.CardsInPlay.Allies)
        {
            TurnManager.instance.CurrPlayer
        };

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
        await PayCostSystem.instance.GetResources(new() { { Resource.Physical, 2 } });

        Detach();
        ScenarioManager.inst.EncounterDeck.Discard(Card);
    }
    #endregion

    public override void Attach()
    {
        attached.Attachments.Add(Card as IAttachment);
        GameStateManager.Instance.OnActivationCompleted += IsTriggerMet;
    }

    
    public override void Detach()
    {
        attached.Attachments.Remove(Card as IAttachment);
        GameStateManager.Instance.OnActivationCompleted -= IsTriggerMet;
    }
}
