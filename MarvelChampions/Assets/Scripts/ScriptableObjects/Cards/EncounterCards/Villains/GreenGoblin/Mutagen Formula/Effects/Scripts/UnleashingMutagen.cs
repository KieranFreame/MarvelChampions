using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Unleashing the Mutagen", menuName = "MarvelChampions/Card Effects/Mutagen Formula/Unleashing the Mutagen")]
public class UnleashingMutagen : EncounterCardEffect
{
    public override async Task OnEnterPlay()
    {
        foreach (var p in TurnManager.Players)
        {
            var data = ScenarioManager.inst.EncounterDeck.Search("Goblin Thrall", false);
            MinionCard thrall = (MinionCard)CreateCardFactory.Instance.CreateCard(data, RevealEncounterCardSystem.Instance.MinionTransform);
            VillainTurnController.instance.MinionsInPlay.Add(thrall);
            await thrall.Effect.OnEnterPlay();
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
                await goblin.Effect.OnEnterPlay();
            }
        }
    }
}
