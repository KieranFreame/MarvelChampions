using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapons Runner", menuName = "MarvelChampions/Card Effects/Klaw/Weapons Runner")]
public class WeaponsRunner : EncounterCardEffect
{
    public override Task Resolve()
    {
        ScenarioManager.inst.Surge(TurnManager.instance.CurrPlayer);
        return Task.CompletedTask;
    }

    public override Task Boost(Action action)
    {
        var card = GameObject.Find("EncounterCards").transform.Find("Weapons Runner");

        card.GetComponentInChildren<MinionCard>().InPlay = true;
        card.transform.SetParent(RevealEncounterCardSystem.Instance.MinionTransform);
        VillainTurnController.instance.MinionsInPlay.Add(card.GetComponentInChildren<MinionCard>());

        return Task.CompletedTask;
    }
}
