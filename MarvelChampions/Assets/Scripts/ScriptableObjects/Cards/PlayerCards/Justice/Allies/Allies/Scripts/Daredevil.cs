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
        (Card as AllyCard).CharStats.ThwartInitiated += ThwartInitiated;
        return Task.CompletedTask;
    }

    private void ThwartInitiated() => ThwartSystem.OnThwartComplete += OnThwartComplete;

    private async void OnThwartComplete()
    {
        ThwartSystem.OnThwartComplete -= OnThwartComplete;

        List<ICharacter> enemies = new() { FindObjectOfType<Villain>() };
        enemies.AddRange(FindObjectsOfType<MinionCard>());
        
        await DamageSystem.Instance.ApplyDamage(new DamageAction(enemies, 1, card:Card));
    }

    public override void OnExitPlay()
    {
        (Card as AllyCard).CharStats.ThwartInitiated -= ThwartInitiated;
    }
}
