using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Sonic Boom (Boost)", menuName = "MarvelChampions/Card Effects/Klaw/Sonic Boom (Boost)")]
public class SonicBoomBoost : EncounterCardEffect
{
    public override Task Resolve()
    {
        if (BoostSystem.Instance.Action is AttackAction)
        {
            var attack = BoostSystem.Instance.Action as AttackAction;
            if (attack.Target is Player)
            {
                var target = attack.Target as Player;
                target.CharStats.Health.OnTakeDamage += OnTakeDamage;
            }
        }

        return Task.CompletedTask;
    }

    private void OnTakeDamage(DamageAction action)
    {
        if (action.Value > 0)
        {
            ((IExhaust)action.DamageTargets[0]).Exhaust();
        }

        (action.DamageTargets[0]).CharStats.Health.OnTakeDamage -= OnTakeDamage;
    }
}
