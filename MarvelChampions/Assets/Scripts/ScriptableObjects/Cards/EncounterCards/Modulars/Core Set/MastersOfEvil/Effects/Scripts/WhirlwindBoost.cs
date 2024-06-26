using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Whirlwind (Boost)", menuName = "MarvelChampions/Card Effects/Masters of Evil/Whirlwind (Boost)")]
public class WhirlwindBoost : EncounterCardEffect
{
    public override async Task Resolve()
    {
        foreach (Player p in TurnManager.Players.Where(x => x.Identity.ActiveIdentity is Hero))
        {
            await DamageSystem.Instance.ApplyDamage(new(p, 1, false, _card, _owner));
        }
    }
}
