using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Tackle", menuName = "MarvelChampions/Card Effects/Protection/Events/Tackle")]
public class Tackle : PlayerCardEffect
{
    public override async Task OnEnterPlay()
    {
        if (_owner.CharStats.Attacker.Stunned)
        {
            _owner.CharStats.Attacker.Stunned = false;
            return;
        }

        List<ICharacter> enemies = new();
        enemies.AddRange(VillainTurnController.instance.MinionsInPlay);

        if (AttackSystem.Instance.Guards.Count == 0)
            enemies.Add(ScenarioManager.inst.ActiveVillain);

        ICharacter target;

        if (enemies.Count == 1)
            target = enemies[0];
        else
            target = await TargetSystem.instance.SelectTarget(enemies);

        target.CharStats.Attacker.Stunned = true;

        if (PayCostSystem.instance.Resources.Contains(Resource.Physical) || PayCostSystem.instance.Resources.Contains(Resource.Wild))
            await _owner.CharStats.InitiateAttack(new(3, target, AttackType.Card, owner: _owner, card: Card));
    }
}
