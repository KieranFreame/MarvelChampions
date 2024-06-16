using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Goblin Knight", menuName = "MarvelChampions/Card Effects/Mutagen Formula/Goblin Knight")]
public class GoblinKnight : EncounterCardEffect
{
    bool resolveBoost = false;

    public override Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
    {
        Card = card;

        AttackSystem.Instance.OnAttackCompleted.Add(IsTriggerMet);
        return Task.CompletedTask;
    }

    private void IsTriggerMet(AttackAction action)
    {
        if (action.Card == Card as ICard)
            EffectManager.Inst.Resolving.Push(this);
    }

    public override async Task Resolve()
    {
        if (resolveBoost)
        {
            EncounterCardData knight = ScenarioManager.inst.EncounterDeck.discardPile.LastOrDefault(x => x.cardName == "Goblin Knight") as EncounterCardData;

            if (knight != null)
            {
                ScenarioManager.inst.EncounterDeck.discardPile.Remove(knight);
                ScenarioManager.inst.EncounterDeck.AddToDeck(knight);
            }

            return;
        }

        EncounterCardData data = ScenarioManager.inst.EncounterDeck.deck[0] as EncounterCardData;
        ScenarioManager.inst.EncounterDeck.Mill();

        if (data is MinionCardData && data.cardTraits.Contains("Goblin"))
        {
            MinionCard goblin = CreateCardFactory.Instance.CreateCard(data, RevealEncounterCardSystem.Instance.MinionTransform) as MinionCard;
            VillainTurnController.instance.MinionsInPlay.Add(goblin);
            await goblin.Effect.OnEnterPlay(_owner, goblin, TurnManager.instance.CurrPlayer);
        }
    }
    public override Task WhenDefeated()
    {
        AttackSystem.Instance.OnAttackCompleted.Remove(IsTriggerMet);
        return Task.CompletedTask;
    }

    #region Boost
    public override Task Boost(Action action)
    {
        EffectManager.Inst.Resolving.Push(this);
        return Task.CompletedTask;
    }
    #endregion
}
