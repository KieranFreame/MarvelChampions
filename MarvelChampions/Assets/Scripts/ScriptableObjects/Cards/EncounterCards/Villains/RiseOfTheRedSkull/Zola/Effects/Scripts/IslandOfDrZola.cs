using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "The Island of Dr Zola", menuName = "MarvelChampions/Card Effects/RotRS/Zola/The Island of Dr Zola")]
public class IslandOfDrZola : EncounterCardEffect
{
    MainSchemeCard MSCard 
    {
        get => Card as MainSchemeCard;
        set
        {
            Card = value;
            Debug.Log(Card);
        }
    }
    public static Counters test;

    public override async Task WhenRevealed(Villain owner, EncounterCard card, Player player)
    {
        MSCard = card as MainSchemeCard;

        //Search Hydra Prison
        var sideScheme = ScenarioManager.inst.EncounterDeck.Search("Hydra Prison");

        SchemeCard HydraPrison = CreateCardFactory.Instance.CreateCard(sideScheme, RevealEncounterCardSystem.Instance.SideSchemeTransform) as SchemeCard;
        ScenarioManager.sideSchemes.Add(HydraPrison);
        await HydraPrison.Effect.WhenRevealed(_owner, HydraPrison, null);

        foreach (var p in TurnManager.Players)
        {
            var minion = ScenarioManager.inst.EncounterDeck.Search("Ultimate Bio-Servant");

            MinionCard BioServant = CreateCardFactory.Instance.CreateCard(minion, RevealEncounterCardSystem.Instance.MinionTransform) as MinionCard;
            VillainTurnController.instance.MinionsInPlay.Add(BioServant);
            await BioServant.Effect.OnEnterPlay(_owner, BioServant, p);
        }

        test = card.gameObject.AddComponent<Counters>();

        MSCard.AfterStepOne.Add(AfterStepOne);
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

            MinionCard card = CreateCardFactory.Instance.CreateCard(minion, RevealEncounterCardSystem.Instance.MinionTransform) as MinionCard;

            VillainTurnController.instance.MinionsInPlay.Add(card);
            card.transform.SetParent(RevealEncounterCardSystem.Instance.MinionTransform);

            await card.Effect.OnEnterPlay(_owner, card, TurnManager.instance.FirstPlayer);

            test.RemoveCounters(3);
        }
    }

    public override Task WhenCompleted()
    {
        ScenarioManager.inst.MainScheme.AfterStepOne.Remove(AfterStepOne);
        return base.WhenCompleted();
    }
}
