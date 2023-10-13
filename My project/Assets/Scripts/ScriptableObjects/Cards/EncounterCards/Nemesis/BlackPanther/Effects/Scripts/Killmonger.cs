using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Killmonger", menuName = "MarvelChampions/Card Effects/Nemesis/Black Panther/Killmonger")]
public class Killmonger : EncounterCardEffect
{
    public override async Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
    {
        _owner = owner;
        Card = card;
        (Card as MinionCard).CharStats.Health.Modifiers.Add(OnTakeDamage);

        await Task.Yield();
    }

    public async Task<DamageAction> OnTakeDamage(DamageAction action)
    {
        if (action.Card != null)
        {
            if (action.Card.CardTraits.Contains("Black Panther"))
            {
                action.Value = 0;
            }
        }

        await Task.Yield();
        return action;
    }

    public override Task WhenDefeated()
    {
        (Card as MinionCard).CharStats.Health.Modifiers.Remove(OnTakeDamage);

        return Task.CompletedTask;
    }
}
