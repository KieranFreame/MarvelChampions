using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Test Subjects", menuName = "MarvelChampions/Card Effects/RotRS/Zola/Test Subjects")]
public class TestSubjects : EncounterCardEffect
{
    public override Task OnEnterPlay()
    {
        VillainTurnController.instance.HazardCount++;
        (_card as SchemeCard).Threat.CurrentThreat *= TurnManager.Players.Count;

        return Task.CompletedTask;
    }

    public override async Task WhenDefeated()
    {
        VillainTurnController.instance.HazardCount--;

        CardData minion;

        do
        {
            minion = ScenarioManager.inst.EncounterDeck.deck[0];
            ScenarioManager.inst.EncounterDeck.Mill(1);
        } while (minion.cardType != CardType.Minion);

        ScenarioManager.inst.EncounterDeck.discardPile.Remove(minion);
        ScenarioManager.inst.EncounterDeck.limbo.Add(minion);

        MinionCard card = CreateCardFactory.Instance.CreateCard(minion, RevealEncounterCardSystem.Instance.MinionTransform) as MinionCard;

        VillainTurnController.instance.MinionsInPlay.Add(card);

        await card.Effect.WhenRevealed(_owner, card, TurnManager.instance.FirstPlayer);
    }
}
