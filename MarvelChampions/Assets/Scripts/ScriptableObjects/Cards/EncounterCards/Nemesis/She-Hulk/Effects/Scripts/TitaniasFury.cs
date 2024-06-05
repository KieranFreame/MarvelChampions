using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;


[CreateAssetMenu(fileName = "Titania's Fury", menuName = "MarvelChampions/Card Effects/Nemesis/She-Hulk/Titania's Fury")]
public class TitaniasFury : EncounterCardEffect
{
    bool didAttack = false;

    public override async Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
    {
        MinionCard titania = VillainTurnController.instance.MinionsInPlay.FirstOrDefault(x => x.CardName == "Titania");

        if (titania == default)
            ScenarioManager.inst.Surge(player);
        else
        {
            if (player.Identity.ActiveIdentity is Hero)
            {

                titania.CharStats.AttackInitiated += AttackInitiated;
                await titania.CharStats.InitiateAttack();

                if (!didAttack)
                {
                    titania.CharStats.Health.CurrentHealth += 6;
                    ScenarioManager.inst.Surge(player);
                }

                titania.CharStats.AttackInitiated -= AttackInitiated;
            }
            else //alter-ego
            {
                titania.CharStats.Health.CurrentHealth += 6;
                ScenarioManager.inst.Surge(player);
            }
        }
    }

    private void AttackInitiated() { didAttack = true; }

    public override async Task Boost(Action action)
    {
        BoostSystem.Instance.DealBoostCards();
        await Task.Yield();
    }
}
