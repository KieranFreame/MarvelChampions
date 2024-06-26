using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Wicked Ambitions", menuName = "MarvelChampions/Card Effects/Mutagen Formula/Wicked Ambitions")]
public class WickedAmbitions : EncounterCardEffect
{
    public override async Task Resolve()
    {
        var player = TurnManager.instance.CurrPlayer;

        for (int i = 0; i < _owner.Stages.Stage * 2; i++)
        {
            EncounterCardData data = ScenarioManager.inst.EncounterDeck.deck[0] as EncounterCardData;

            if (data.cardTraits.Contains("Goblin"))
            {
                int decision = await ChooseEffectUI.ChooseEffect(new List<string>() { "Take 3 damage", $"Put {data.cardName} into play engaged with you" });

                if (decision == 1)
                {
                    player.CharStats.Health.CurrentHealth -= 3;
                }
                else
                {
                    MinionCard goblin = CreateCardFactory.Instance.CreateCard(data, RevealEncounterCardSystem.Instance.MinionTransform) as MinionCard;
                    VillainTurnController.instance.MinionsInPlay.Add(goblin);
                    await goblin.Effect.OnEnterPlay();
                }
            }
            else
            {
                ScenarioManager.inst.EncounterDeck.Mill(1);
            }
        }
    }
}
