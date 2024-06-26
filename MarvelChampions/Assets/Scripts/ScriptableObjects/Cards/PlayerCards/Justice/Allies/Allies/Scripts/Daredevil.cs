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
        GameStateManager.Instance.OnActivationCompleted += IsTriggerMet;
        return Task.CompletedTask;
    }

    private void IsTriggerMet(Action action)
    {
        if (action is ThwartAction && action.Owner.Name == "Daredevil")
            EffectManager.Inst.Responding.Add(this);
    }

    public override async Task Resolve()
    {
        List<ICharacter> enemies = new() { ScenarioManager.inst.ActiveVillain };
        enemies.AddRange(VillainTurnController.instance.MinionsInPlay);

        await DamageSystem.Instance.ApplyDamage(new DamageAction(enemies, 1, card: Card));
    }

    public override void OnExitPlay()
    {
        GameStateManager.Instance.OnActivationCompleted -= IsTriggerMet;
    }
}
