using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace CoreSet
{
    [CreateAssetMenu(fileName = "Radioactive Man", menuName = "MarvelChampions/Card Effects/Masters of Evil/Radioactive Man")]
    public class RadioactiveMan : EncounterCardEffect
    {
        public override Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
        {
            _owner = owner;
            Card = card;

            (Card as MinionCard).CharStats.AttackInitiated += AttackInitiated;

            return Task.CompletedTask;
        }

        private void AttackInitiated() { AttackSystem.Instance.OnAttackCompleted.Add(AttackCompleted); }

        private Task AttackCompleted(Action action)
        {
            AttackSystem.Instance.OnAttackCompleted.Remove(AttackCompleted);

            var attack = action as AttackAction;

            Player p = (attack.Target is not Player) ? (attack.Target as AllyCard).Owner : attack.Target as Player;

            Debug.Log("Discarding 1 Card from your hand");
            var pCard = p.Hand.cards[Random.Range(0, p.Hand.cards.Count)];

            Debug.Log("Discarded " + pCard.CardName);

            p.Hand.Remove(pCard);
            p.Deck.Discard(pCard);

            return Task.CompletedTask;
        }

        public override Task Boost(Action action)
        {
            Player p;

            if (action is AttackAction)
            {
                var attack = action as AttackAction;
                p = (attack.Target is not Player) ? (attack.Target as AllyCard).Owner : attack.Target as Player;
            }
            else //scheme
            {
                p = TurnManager.instance.CurrPlayer;
            }
            

            Debug.Log("Discarding 1 Card from your hand");
            var pCard = p.Hand.cards[Random.Range(0, p.Hand.cards.Count)];

            Debug.Log("Discarded " + pCard.CardName);

            p.Hand.Remove(pCard);
            p.Deck.Discard(pCard);

            return Task.CompletedTask;
        }

        public override Task WhenDefeated()
        {
            (Card as MinionCard).CharStats.AttackInitiated -= AttackInitiated;

            return Task.CompletedTask;
        }
    }
}

