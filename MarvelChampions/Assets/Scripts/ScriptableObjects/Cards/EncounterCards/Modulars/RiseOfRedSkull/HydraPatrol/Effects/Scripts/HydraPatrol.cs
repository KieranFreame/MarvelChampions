using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Hydra Patrol", menuName = "MarvelChampions/Card Effects/Modulars/RotRS/Hydra Patrol/Hydra Patrol")]
public class HydraPatrol : EncounterCardEffect
{
    public override Task OnEnterPlay()
    {
        (_card as SchemeCard).Threat.CurrentThreat *= TurnManager.Players.Count;

        ThwartSystem.Instance.Crisis.Add(_card as SchemeCard);
        return Task.CompletedTask;
    }

    public override async Task WhenDefeated()
    {
        ThwartSystem.Instance.Crisis.Remove(Card as SchemeCard);

        List<MinionCardData> hydra = ScenarioManager.inst.EncounterDeck.deck.Where(x => x.cardTraits.Contains("Hydra")).Cast<MinionCardData>().ToList();
        hydra.AddRange(ScenarioManager.inst.EncounterDeck.discardPile.Where(x => x.cardTraits.Contains("Hydra")).Cast<MinionCardData>().ToList());

        List<EncounterCard> cards = CardViewerUI.inst.EnablePanel(hydra.Cast<CardData>().ToList()).Cast<EncounterCard>().ToList();
        var minion = await TargetSystem.instance.SelectTarget(cards);

        RevealEncounterCardSystem.Instance.MoveCard(minion);

        CardViewerUI.inst.DisablePanel();

        ScenarioManager.inst.EncounterDeck.limbo.Add(minion.Data);
        ScenarioManager.inst.EncounterDeck.deck.Remove(minion.Data);

        ScenarioManager.inst.EncounterDeck.Shuffle();

        await minion.Effect.OnEnterPlay();
    }
}
