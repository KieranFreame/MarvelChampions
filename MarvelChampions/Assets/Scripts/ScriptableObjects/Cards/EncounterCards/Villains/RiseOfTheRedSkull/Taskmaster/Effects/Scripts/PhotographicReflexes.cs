using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Photographic Reflexes", menuName = "MarvelChampions/Card Effects/RotRS/Taskmaster/Photographic Reflexes")]
public class PhotographicReflexes : AttachmentCardEffect
{
    public override Task OnEnterPlay()
    {
        Attach();
        return Task.CompletedTask;
    }

    private async Task<DamageAction> CancelDamage(DamageAction action)
    {
        if (action.IsAttack)
        {
            await DamageSystem.Instance.ApplyDamage(new(action.Owner, action.Value, false, _card, _owner));
            action.Value = 0;

            Detach();
            ScenarioManager.inst.EncounterDeck.Discard(Card);
        }

        return action;
    }


    public override void Attach()
    {
        _owner.CharStats.Health.Modifiers.Add(CancelDamage);
    }
    
    public override void Detach()
    {
        _owner.CharStats.Health.Modifiers.Remove(CancelDamage);
    }
}
