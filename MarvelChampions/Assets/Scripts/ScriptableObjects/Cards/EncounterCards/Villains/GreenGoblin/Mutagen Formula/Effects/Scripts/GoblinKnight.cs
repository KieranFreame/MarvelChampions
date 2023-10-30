using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Goblin Knight", menuName = "MarvelChampions/Card Effects/Mutagen Formula/Goblin Knight")]
public class GoblinKnight : EncounterCardEffect
{
    public override Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
    {
        Card = card;

        (Card as MinionCard).CharStats.AttackInitiated += AttackInitiated;
        return Task.CompletedTask;
    }

    private void AttackInitiated()
    {
        AttackSystem.Instance.OnAttackCompleted.Add(AttackCompleted);
    }

    private async Task AttackCompleted(AttackAction action)
    {
        AttackSystem.Instance.OnAttackCompleted.Remove(AttackCompleted);

        EncounterCardData data = ScenarioManager.inst.EncounterDeck.deck[0] as EncounterCardData;
        ScenarioManager.inst.EncounterDeck.Mill();

        if (data is MinionCardData && data.cardTraits.Contains("Goblin"))
        {
            MinionCard goblin = CreateCardFactory.Instance.CreateCard(data, GameObject.Find("MinionTransform").transform) as MinionCard;
            VillainTurnController.instance.MinionsInPlay.Add(goblin);
            await goblin.Effect.OnEnterPlay(_owner, goblin, (action.Target is Player) ? action.Target as Player : (action.Target as AllyCard).Owner);
        }
    }
    public override Task WhenDefeated()
    {
        (Card as MinionCard).CharStats.AttackInitiated -= AttackInitiated;
        return Task.CompletedTask;
    }

    #region Boost
    private Task BoostCompleted(Action action)
    {
        EncounterCardData knight = ScenarioManager.inst.EncounterDeck.discardPile.LastOrDefault(x => x.cardName == "Goblin Knight") as EncounterCardData;

        if (knight != null)
        {
            ScenarioManager.inst.EncounterDeck.discardPile.Remove(knight);
            ScenarioManager.inst.EncounterDeck.AddToDeck(knight);
        }

        return Task.CompletedTask;
    }

    public override Task Boost(Action action)
    {
        if (action is SchemeAction)
            SchemeSystem.Instance.SchemeComplete.Add(BoostCompleted);
        else
            AttackSystem.Instance.OnAttackCompleted.Add(BoostCompleted);

        return Task.CompletedTask;
    }
    #endregion
}
