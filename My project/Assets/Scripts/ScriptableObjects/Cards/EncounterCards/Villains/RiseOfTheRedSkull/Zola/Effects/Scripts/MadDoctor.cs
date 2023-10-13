using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "The Mad Doctor", menuName = "MarvelChampions/Card Effects/RotRS/Zola/The Mad Doctor")]
public class MadDoctor : EncounterCardEffect
{
    public static Counters test;

    public override async Task WhenRevealed(Villain owner, EncounterCard card, Player player)
    {
        List<EncounterCardData> data = new();
        data.AddRange(ScenarioManager.inst.EncounterDeck.deck.Where(x => x is MinionCardData).Cast<EncounterCardData>().ToList());
        data.AddRange(ScenarioManager.inst.EncounterDeck.discardPile.Where(x => x is MinionCardData).Cast<EncounterCardData>().ToList());

        MinionCard minion;

        List<MinionCard> minions = CardViewerUI.inst.EnablePanel(data.Cast<CardData>().ToList()).Cast<MinionCard>().ToList();

        foreach (var p in TurnManager.Players)
        {
            minion = await TargetSystem.instance.SelectTarget(minions);

            VillainTurnController.instance.MinionsInPlay.Add(minion);
            minion.transform.SetParent(GameObject.Find("MinionTransform").transform);

            await minion.Effect.WhenRevealed(_owner, minion, p);
        }

        CardViewerUI.inst.DisablePanel();

        test = card.gameObject.AddComponent<Counters>();
        test.AddCounters(IslandOfDrZola.test.CountersLeft);

        (card as MainSchemeCard).AfterStepOne.Add(AfterStepOne);
    }

    private async Task AfterStepOne()
    {
        test.AddCounters(1);

        if (test.CountersLeft >= 3)
        {
            CardData minion;

            do
            {
                minion = ScenarioManager.inst.EncounterDeck.deck[0];
                ScenarioManager.inst.EncounterDeck.Mill(1);
            } while (minion.cardType != CardType.Minion);

            ScenarioManager.inst.EncounterDeck.discardPile.Remove(minion);
            ScenarioManager.inst.EncounterDeck.limbo.Add(minion);

            MinionCard card = CreateCardFactory.Instance.CreateCard(minion, GameObject.Find("MinionTransform").transform) as MinionCard;

            VillainTurnController.instance.MinionsInPlay.Add(card);
            card.transform.SetParent(GameObject.Find("MinionTransform").transform);

            await card.Effect.OnEnterPlay(_owner, card, TurnManager.instance.FirstPlayer);

            test.RemoveCounters(3);
        }
    }
}
