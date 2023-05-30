using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Daredevil", menuName = "MarvelChampions/Card Effects/Justice/Daredevil")]
public class Daredevil : PlayerCardEffect
{
    /// <summary>
    /// After Daredevil thwarts, deal 1 damage to an enemy.
    /// </summary>

    public override void OnEnterPlay(Player owner, Card card)
    {
        _owner = owner;
        _card = card;

        ThwartSystem.OnThwartComplete += OnThwartComplete;
    }

    private void OnThwartComplete()
    {
        if (ThwartSystem.instance.Action.Owner != _card)
            return;

        List<ICharacter> enemies = new() { FindObjectOfType<Villain>() };
        enemies.AddRange(FindObjectsOfType<MinionCard>());
        
        _card.StartCoroutine(DamageSystem.instance.ApplyDamage(new DamageAction(enemies, 1)));
    }

    public override void OnExitPlay()
    {
        ThwartSystem.OnThwartComplete -= OnThwartComplete;
    }
}
