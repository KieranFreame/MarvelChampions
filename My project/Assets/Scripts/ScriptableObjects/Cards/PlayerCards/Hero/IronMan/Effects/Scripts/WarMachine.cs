using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace CoreSet
{

    [CreateAssetMenu(fileName = "War Machine", menuName = "MarvelChampions/Card Effects/Iron Man/War Machine")]
    public class WarMachine : PlayerCardEffect
    {
        List<ICharacter> enemies = new();

        public override bool CanBePlayed()
        {
            foreach (AllyCard a in _owner.CardsInPlay.Allies)
            {
                if (a.CardName == "War Machine" && (a.Data as AllyCardData).alterEgo == "James Rhodes")
                    return false;
            }

            return base.CanBePlayed();
        }

        public override bool CanActivate()
        {
            if (Card.Exhausted || (Card as AllyCard).CharStats.Health.CurrentHealth < 2)
                return false;

            enemies.Clear();
            enemies.Add(FindObjectOfType<Villain>());
            enemies.AddRange(VillainTurnController.instance.MinionsInPlay);

            return enemies.Count > 0;
        }

        public override async Task Activate()
        {
            Card.Exhaust();
            (Card as AllyCard).CharStats.Health.TakeDamage(2);

            await DamageSystem.instance.ApplyDamage(new(enemies, 1, true));
        }
    }
}

