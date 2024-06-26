using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;


[CreateAssetMenu(fileName = "Titania's Fury", menuName = "MarvelChampions/Card Effects/Nemesis/She-Hulk/Titania's Fury")]
public class TitaniasFury : EncounterCardEffect
{
    public override async Task Resolve()
    {
        var player = TurnManager.instance.CurrPlayer;
        MinionCard titania = VillainTurnController.instance.MinionsInPlay.FirstOrDefault(x => x.CardName == "Titania");

        if (titania == default)
            ScenarioManager.inst.Surge(player);
        else
        {
            if (player.Identity.ActiveIdentity is AlterEgo || !await titania.CharStats.InitiateAttack())
            {
                titania.CharStats.Health.CurrentHealth += titania.CharStats.Health.BaseHP;
                ScenarioManager.inst.Surge(player);
            }
        }
    }

    public override async Task Boost(Action action)
    {
        BoostSystem.Instance.DealBoostCards();
        await Task.Yield();
    }
}
