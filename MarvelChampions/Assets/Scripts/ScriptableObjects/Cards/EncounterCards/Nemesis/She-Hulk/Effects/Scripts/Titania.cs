using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Titania", menuName = "MarvelChampions/Card Effects/Nemesis/She-Hulk/Titania")]
public class Titania : EncounterCardEffect
{
    public override async Task OnEnterPlay()
    {
        (_card as MinionCard).CharStats.Attacker.CurrentAttack = (_card as MinionCard).CharStats.Health.CurrentHealth;

        (_card as MinionCard).CharStats.Health.HealthChanged += HealthChanged;
        await Task.Yield();
    }

    private void HealthChanged()
    {
        (Card as MinionCard).CharStats.Attacker.CurrentAttack = (Card as MinionCard).CharStats.Health.CurrentHealth;
    }

    public override Task WhenDefeated()
    {
        (Card as MinionCard).CharStats.Health.HealthChanged -= HealthChanged;
        return Task.CompletedTask;
    }
}
