using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Legions Of Hydra", menuName = "MarvelChampions/Card Effects/Legions of Hydra/Legions Of Hydra")]
public class LegionsOfHydra : EncounterCardEffect
{
    public override async Task OnEnterPlay()
    {
        MinionCard madame = VillainTurnController.instance.MinionsInPlay.FirstOrDefault(x => x.CardName == "Madame Hydra");

        if (madame == default) //not in play
        {
            EncounterCardData data = ScenarioManager.inst.EncounterDeck.Search("Madame Hydra", true) as EncounterCardData;
            
            if (data != null)
            {
                madame = CreateCardFactory.Instance.CreateCard(data, RevealEncounterCardSystem.Instance.MinionTransform) as MinionCard;
                VillainTurnController.instance.MinionsInPlay.Add(madame);
                await madame.Effect.OnEnterPlay();
            }
        }

        int hydra = VillainTurnController.instance.MinionsInPlay.Where(x => x.CardTraits.Contains("Hydra")).Count();

        if (ScenarioManager.inst.ActiveVillain.VillainTraits.Contains("Hydra")) hydra++;

        (_card as SchemeCard).Threat.GainThreat(hydra*2);

        VillainTurnController.instance.HazardCount++;
    }

    public override Task WhenDefeated()
    {
        VillainTurnController.instance.HazardCount--;
        return Task.CompletedTask;
    }
}
