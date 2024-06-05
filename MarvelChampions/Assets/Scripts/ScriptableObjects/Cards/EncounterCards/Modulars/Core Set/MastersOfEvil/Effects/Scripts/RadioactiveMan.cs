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

            AttackSystem.Instance.OnAttackCompleted.Add(IsTriggerMet);

            return Task.CompletedTask;
        }

        private void IsTriggerMet(AttackAction action)
        {
            if (action.Card.CardName == "Radioactive Man")
                EffectResolutionManager.Instance.ResolvingEffects.Push(this);
        }

        public override Task Resolve()
        {
            Player p = TurnManager.instance.CurrPlayer;

            Debug.Log("Discarding 1 Card from your hand");
            var pCard = p.Hand.cards[Random.Range(0, p.Hand.cards.Count)];

            Debug.Log("Discarded " + pCard.CardName);

            p.Hand.Remove(pCard);
            p.Deck.Discard(pCard);

            return Task.CompletedTask;
        }

        public override Task Boost(Action action)
        {
            Player p = TurnManager.instance.CurrPlayer;

            Debug.Log("Discarding 1 Card from your hand");
            var pCard = p.Hand.cards[Random.Range(0, p.Hand.cards.Count)];

            Debug.Log("Discarded " + pCard.CardName);

            p.Hand.Remove(pCard);
            p.Deck.Discard(pCard);

            return Task.CompletedTask;
        }

        public override Task WhenDefeated()
        {
            AttackSystem.Instance.OnAttackCompleted.Remove(IsTriggerMet);

            return Task.CompletedTask;
        }
    }
}

