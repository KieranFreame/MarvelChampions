using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Superhuman Strength", menuName = "MarvelChampions/Card Effects/She-Hulk/Superhuman Strength")]
public class SuperhumanStrength : PlayerCardEffect
{
    public override Task OnEnterPlay()
    {
        _owner.CharStats.Attacker.CurrentAttack += 2;
        GameStateManager.Instance.OnActivationCompleted += IsTriggerMet;

        return Task.CompletedTask;
    }

    public void IsTriggerMet(Action action)
    {
        if (action.Owner.Name == "She-Hulk")
            EffectManager.Inst.Responding.Add(this);
    }

    public override Task Resolve()
    {
        var target = AttackSystem.Instance.Action.Target;

        if (target != null)
           target.CharStats.Attacker.Stunned = true;
        

        _owner.CharStats.Attacker.CurrentAttack -= 2;
        GameStateManager.Instance.OnActivationCompleted -= IsTriggerMet;

        _owner.CardsInPlay.Permanents.Remove(_card);
        _owner.Deck.Discard(_card);

        return Task.CompletedTask;
    }
}
