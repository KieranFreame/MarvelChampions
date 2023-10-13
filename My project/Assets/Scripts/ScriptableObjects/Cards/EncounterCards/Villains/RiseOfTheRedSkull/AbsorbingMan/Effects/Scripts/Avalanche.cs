using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Avalanche!", menuName = "MarvelChampions/Card Effects/RotRS/Absorbing Man/Avalanche!")]
public class Avalanche : EncounterCardEffect
{
    public override async Task WhenRevealed(Villain owner, EncounterCard card, Player player)
    {
        (card as SchemeCard).Threat.CurrentThreat *= TurnManager.Players.Count;

        foreach (var p in TurnManager.Players)
        {
            int choice = await ChooseEffectUI.ChooseEffect(new() { "Spend an Energy Resource", $"Take {(NoneShallPass.delay.CountersLeft >= 5 ? 3 : 2)} indirect damage"});

            if (choice == 1)
            {
                await PayCostSystem.instance.GetResources(Resource.Energy, 1);
                continue;
            }
            else
            {
                List<ICharacter> targets = new() { p };
                targets.AddRange(p.CardsInPlay.Allies);

                await IndirectDamageHandler.inst.HandleIndirectDamage(targets, NoneShallPass.delay.CountersLeft >= 5 ? 3 : 2);
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
