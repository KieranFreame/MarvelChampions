using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Repulsor Blast", menuName = "MarvelChampions/Card Effects/Iron Man/Repulsor Blast")]
public class RepulsorBlast : PlayerCardEffect
{
    readonly List<ICharacter> enemies = new();

    public override bool CanBePlayed()
    {
        if (base.CanBePlayed())
        {
            enemies.Clear();

            enemies.Add(FindObjectOfType<Villain>());
            enemies.AddRange(FindObjectsOfType<MinionCard>());

            return enemies.Count > 0;
        }

        return false;
    }

    public override async Task OnEnterPlay()
    {
        if (_owner.CharStats.Attacker.Stunned)
        {
            _owner.CharStats.Attacker.Stunned = false;
            await Task.Yield();
            return;
        }

        var _target = await TargetSystem.instance.SelectTarget(enemies);
        await DamageSystem.Instance.ApplyDamage(new(_target, 1, card: Card));

        int damage = 0;

        for (int i = 0; i < 5; i++)
        {
            PlayerCardData data = _owner.Deck.deck[0] as PlayerCardData;

            Debug.Log("Top card is " + data.cardName + ".");

            foreach (Resource r in data.cardResources)
                if (r == Resource.Energy) damage += 2;

            _owner.Deck.Mill(1);
        }

        var attack = new DamageAction(_target, damage, card:Card);
        await DamageSystem.Instance.ApplyDamage(attack);
    }
}
