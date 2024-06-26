using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Baron Zemo", menuName = "MarvelChampions/Card Effects/Nemesis/Captain America/Baron Zemo")]
public class BaronZemo : EncounterCardEffect
{
    Player affected;

    public override async Task OnEnterPlay()
    {
        affected = TurnManager.instance.CurrPlayer;

        //Quickstrike
        await (_card as MinionCard).CharStats.InitiateAttack();

        affected.CanThwart = false;
    }

    public override Task WhenDefeated()
    {
        affected.CanThwart = true;
        return base.WhenDefeated();
    }
}
