using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
            if (base.CanBePlayed())
            {
                foreach (AllyCard a in _owner.CardsInPlay.Allies)
                {
                    return _owner.CardsInPlay.Allies.Any(x => (x.Data as AllyCardData).alterEgo == "James Rhodes");
                }
            }

            return false;
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
            (Card as AllyCard).CharStats.Health.CurrentHealth -= 2;

            await DamageSystem.Instance.ApplyDamage(new(enemies, 1, true, card: Card));
        }
    }
}

