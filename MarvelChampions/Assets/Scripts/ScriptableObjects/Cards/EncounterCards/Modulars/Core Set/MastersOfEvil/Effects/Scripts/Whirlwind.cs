using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Whirlwind", menuName = "MarvelChampions/Card Effects/Masters of Evil/Whirlwind")]
public class Whirlwind : EncounterCardEffect
{
    public override async Task Boost(Action action)
    {
        foreach (Player p in TurnManager.Players.Where(x => x.Identity.ActiveIdentity is Hero))
        {
            await DamageSystem.Instance.ApplyDamage(new(p, 1, card:GameObject.Find("Whirlwind").GetComponent<EncounterCard>(), owner:action.Owner));
        }
    }
}
