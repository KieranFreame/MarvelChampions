using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Daredevil", menuName = "MarvelChampions/Card Effects/Justice/Daredevil")]
public class Daredevil : PlayerCardEffect
{
    /// <summary>
    /// After Daredevil thwarts, deal 1 damage to an enemy.
    /// </summary>

    public override Task OnEnterPlay()
    {
        ThwartSystem.Instance.OnThwartComplete.Add(IsTriggerMet);
        return Task.CompletedTask;
    }

    private void IsTriggerMet(ThwartAction action)
    {
        if (action.Owner == Card as ICharacter)
            EffectManager.Inst.Resolving.Push(this);
    }

    public override async Task Resolve()
    {
        List<ICharacter> enemies = new() { ScenarioManager.inst.ActiveVillain };
        enemies.AddRange(VillainTurnController.instance.MinionsInPlay);

        await DamageSystem.Instance.ApplyDamage(new DamageAction(enemies, 1, card: Card));
    }

    public override void OnExitPlay()
    {
        ThwartSystem.Instance.OnThwartComplete.Remove(IsTriggerMet);
    }
}
