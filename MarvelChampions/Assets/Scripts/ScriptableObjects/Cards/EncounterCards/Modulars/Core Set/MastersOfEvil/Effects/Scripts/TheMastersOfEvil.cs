using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "The Masters of Evil", menuName = "MarvelChampions/Card Effects/Masters of Evil/Masters of Evil")]
public class TheMastersOfEvil : EncounterCardEffect
{
    public override Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
    {
        EncounterCardData data;

        do
        {
            data = ScenarioManager.inst.EncounterDeck.deck[0] as EncounterCardData;
            ScenarioManager.inst.EncounterDeck.Mill(1);
        } while (data is not MinionCardData && !data.cardTraits.Contains("Masters of Evil"));

        ScenarioManager.inst.EncounterDeck.discardPile.Remove(data);
        ScenarioManager.inst.EncounterDeck.limbo.Add(data);

        MinionCard minion = CreateCardFactory.Instance.CreateCard(data, RevealEncounterCardSystem.Instance.MinionTransform) as MinionCard;
        VillainTurnController.instance.MinionsInPlay.Add(minion);

        (card as SchemeCard).Threat.CurrentThreat *= TurnManager.Players.Count;
        ScenarioManager.inst.MainScheme.Threat.Acceleration++;

        return Task.CompletedTask;
    }

    public override Task WhenDefeated()
    {
        ScenarioManager.inst.MainScheme.Threat.Acceleration--;

        return Task.CompletedTask;
    }
}
