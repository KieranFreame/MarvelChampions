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

            GameStateManager.Instance.OnActivationCompleted += CanRespond;

            return Task.CompletedTask;
        }

        private async void CanRespond(Action action)
        {
            if (action is not AttackAction || ((AttackAction)action).Card.CardName != "Radioactive Man")
                return;

            await EffectManager.Inst.AddEffect(_card, this);
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
            GameStateManager.Instance.OnActivationCompleted -= CanRespond;
            return Task.CompletedTask;
        }
    }
}

