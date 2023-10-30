using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "None Shall Pass", menuName = "MarvelChampions/Card Effects/RotRS/Absorbing Man/None Shall Pass")]
public class NoneShallPass : EncounterCardEffect
{
    public static Counters delay { get; private set; }
    public static EncounterCard CurrentEnvironment { get; private set; }
    public static event Action<EncounterCard> EnvironmentChanged;

    public override async Task WhenRevealed(Villain owner, EncounterCard card, Player player)
    {
        delay = card.gameObject.AddComponent<Counters>();

        EncounterCardData data;

        do
        {
            data = ScenarioManager.inst.EncounterDeck.deck[0] as EncounterCardData;
            ScenarioManager.inst.EncounterDeck.Mill(1);
        } while (data.cardType != CardType.Environment);

        ScenarioManager.inst.EncounterDeck.limbo.Add(data);
        ScenarioManager.inst.EncounterDeck.discardPile.Remove(data);

        CurrentEnvironment = CreateCardFactory.Instance.CreateCard(data, GameObject.Find("AttachmentTransform").transform) as EncounterCard;
        await CurrentEnvironment.Effect.OnEnterPlay(owner, CurrentEnvironment, player);

        ScenarioManager.inst.EncounterDeck.AddToDeck(ScenarioManager.inst.EncounterDeck.discardPile);
        ScenarioManager.inst.EncounterDeck.discardPile.Clear();

        (card as MainSchemeCard).AfterStepOne.Add(AfterStepOne);
        RevealEncounterCardSystem.OnEncounterCardRevealed += EncounterCardRevealed;
    }

    public async void EncounterCardRevealed(EncounterCard arg0)
    {
        if (arg0.CardType == CardType.Environment)
        {
            await CurrentEnvironment.Effect.OnExitPlay();
            ScenarioManager.inst.EncounterDeck.Discard(CurrentEnvironment);
            EnvironmentChanged?.Invoke(arg0);
            CurrentEnvironment = arg0;
        }
    }

    private Task AfterStepOne()
    {
        delay.AddCounters(1);
        return Task.CompletedTask;
    }


}
