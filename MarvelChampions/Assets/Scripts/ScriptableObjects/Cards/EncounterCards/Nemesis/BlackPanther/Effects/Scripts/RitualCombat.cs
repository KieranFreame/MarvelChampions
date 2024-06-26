using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;


[CreateAssetMenu(fileName = "Ritual Combat", menuName = "MarvelChampions/Card Effects/Nemesis/Black Panther/Ritual Combat")]
public class RitualCombat : EncounterCardEffect
{
    public override async Task Resolve()
    {
        ScenarioManager.inst.EncounterDeck.Mill(1);

        int boostCount = (ScenarioManager.inst.EncounterDeck.discardPile.Last() as EncounterCardData).boostIcons + 1;
        var player = TurnManager.instance.CurrPlayer;

        if (player.Identity.ActiveIdentity is Hero)
        {
            int decision = await ChooseEffectUI.ChooseEffect(new List<string>() { $"Deal {boostCount} damage to your hero", $"Place {boostCount} threat on the main scheme" });

            if (decision == 1)
            {
                player.CharStats.Health.TakeDamage(new(player, boostCount, _card));
                return;
            }
        }

        ScenarioManager.inst.MainScheme.Threat.GainThreat(boostCount);
    }
}
