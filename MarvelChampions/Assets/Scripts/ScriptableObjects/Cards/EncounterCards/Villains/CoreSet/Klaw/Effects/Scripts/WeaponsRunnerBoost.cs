using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapons Runner (Boost)", menuName = "MarvelChampions/Card Effects/Klaw/Weapons Runner (Boost)")]
public class WeaponsRunnerBoost : EncounterCardEffect
{
    public override Task Resolve()
    {
        var card = GameObject.Find("EncounterCards").transform.Find("Weapons Runner");

        card.GetComponentInChildren<MinionCard>().InPlay = true;
        card.transform.SetParent(RevealEncounterCardSystem.Instance.MinionTransform);
        VillainTurnController.instance.MinionsInPlay.Add(card.GetComponentInChildren<MinionCard>());

        return Task.CompletedTask;
    }
}
