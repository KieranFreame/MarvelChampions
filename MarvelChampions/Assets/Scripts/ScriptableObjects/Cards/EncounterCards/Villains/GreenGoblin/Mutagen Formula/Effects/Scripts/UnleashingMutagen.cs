using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Unleashing the Mutagen", menuName = "MarvelChampions/Card Effects/Mutagen Formula/Unleashing the Mutagen")]
public class UnleashingMutagen : EncounterCardEffect
{
    public override async Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
    {
        _owner = owner;
        Card = card;

        foreach (var p in TurnManager.Players)
        {
            MinionCardData data = ScenarioManager.inst.EncounterDeck.Search("Goblin Thrall", false) as MinionCardData;
            MinionCard thrall = CreateCardFactory.Instance.CreateCard(data, RevealEncounterCardSystem.Instance.MinionTransform) as MinionCard;
            VillainTurnController.instance.MinionsInPlay.Add(thrall);
            await thrall.Effect.OnEnterPlay(_owner, thrall, p);
        }
    }

    public override async Task WhenCompleted()
    {
        if (!VillainTurnController.instance.MinionsInPlay.Any(x => x.CardTraits.Contains("Goblin")))
        {
            EncounterCardData data;
            int i = 0;

            do
            {
                data = ScenarioManager.inst.EncounterDeck.deck[0] as EncounterCardData;
                ScenarioManager.inst.EncounterDeck.Mill(1);
                i++;
            } while (data is not MinionCardData && i < 3);

            if (data != default)
            {
                MinionCard goblin = CreateCardFactory.Instance.CreateCard(data, RevealEncounterCardSystem.Instance.MinionTransform) as MinionCard;
                VillainTurnController.instance.MinionsInPlay.Add(goblin);
                await goblin.Effect.OnEnterPlay(_owner, goblin, TurnManager.instance.CurrPlayer);
            }
        }
    }
}
