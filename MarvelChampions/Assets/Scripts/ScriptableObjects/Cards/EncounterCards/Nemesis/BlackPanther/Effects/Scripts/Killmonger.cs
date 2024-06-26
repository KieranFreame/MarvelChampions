using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Killmonger", menuName = "MarvelChampions/Card Effects/Nemesis/Black Panther/Killmonger")]
public class Killmonger : EncounterCardEffect
{
    public override async Task OnEnterPlay()
    {
        (Card as MinionCard).CharStats.Health.Modifiers.Add(OnTakeDamage);
        await Task.Yield();
    }

    public Task<DamageAction> OnTakeDamage(DamageAction action)
    {
        if (action.Card != null)
        {
            if (action.Card.CardTraits.Contains("Black Panther"))
            {
                action.Value = 0;
            }
        }

        
        return Task.FromResult(action);
    }

    public override Task WhenDefeated()
    {
        (Card as MinionCard).CharStats.Health.Modifiers.Remove(OnTakeDamage);

        return Task.CompletedTask;
    }
}
