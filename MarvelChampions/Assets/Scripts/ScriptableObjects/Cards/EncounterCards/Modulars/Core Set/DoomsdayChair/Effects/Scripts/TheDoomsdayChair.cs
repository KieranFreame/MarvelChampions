using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "The Doomsday Chair", menuName = "MarvelChampions/Card Effects/The Doomsday Chair/The Doomsday Chair")]
public class TheDoomsdayChair : EncounterCardEffect
{
    public override async Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
    {
        MinionCard modok = VillainTurnController.instance.MinionsInPlay.FirstOrDefault(x => x.CardName == "MODOK");

        if (modok == default) //not in play
        {
            EncounterCardData data = ScenarioManager.inst.EncounterDeck.Search("MODOK", true) as EncounterCardData;

            if (data != null)
            {
                modok = CreateCardFactory.Instance.CreateCard(data, RevealEncounterCardSystem.Instance.MinionTransform) as MinionCard;
                VillainTurnController.instance.MinionsInPlay.Add(modok);
                await modok.Effect.OnEnterPlay(owner, modok, player);
            }
        }

        ScenarioManager.inst.MainScheme.Threat.Acceleration++;
    }

    public override Task WhenDefeated()
    {
        ScenarioManager.inst.MainScheme.Threat.Acceleration--;
        return Task.CompletedTask;
    }
}
