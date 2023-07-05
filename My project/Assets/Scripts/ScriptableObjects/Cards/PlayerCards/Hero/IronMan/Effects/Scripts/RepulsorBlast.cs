using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Repulsor Blast", menuName = "MarvelChampions/Card Effects/Iron Man/Repulsor Blast")]
public class RepulsorBlast : PlayerCardEffect
{
    List<ICharacter> enemies = new();

    public override bool CanBePlayed()
    {
        enemies.Clear();

        enemies.Add(FindObjectOfType<Villain>());
        enemies.AddRange(FindObjectsOfType<MinionCard>());

        if (enemies.Count == 0) return false;

        return true;
    }

    public override async Task OnEnterPlay()
    {
        if (_owner.CharStats.Attacker.Stunned)
        {
            _owner.CharStats.Attacker.Stunned = false;
            await Task.Yield();
            return;
        }

        var _target = await TargetSystem.instance.SelectTarget(enemies, true);
        await DamageSystem.instance.ApplyDamage(new(_target, 1));

        int damage = 0;

        for (int i = 0; i < 5; i++)
        {
            PlayerCardData data = _owner.Deck.deck[0] as PlayerCardData;

            Debug.Log("Top card is " + data.cardName + ".");

            foreach (Resource r in data.cardResources)
                if (r == Resource.Energy) damage += 2;

            _owner.Deck.Mill(1);
        }

        var attack = new DamageAction(_target, damage);
        await DamageSystem.instance.ApplyDamage(attack);
    }
}
