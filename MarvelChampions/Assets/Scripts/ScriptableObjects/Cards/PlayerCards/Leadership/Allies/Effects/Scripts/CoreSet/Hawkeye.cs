using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;
using UnityEngine;

namespace CoreSet
{
    [CreateAssetMenu(fileName = "Hawkeye (Clint Barton)", menuName = "MarvelChampions/Card Effects/Leadership/Hawkeye (Clint Barton)")]
    public class Hawkeye : PlayerCardEffect
    {
        Counters arrows;

        public override async Task OnEnterPlay()
        {
            arrows = Card.gameObject.AddComponent<Counters>();
            arrows.AddCounters(4);

            VillainTurnController.instance.MinionsInPlay.CollectionChanged += MinionAdded;

            await Task.Yield();
        }

        private async void MinionAdded(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action is NotifyCollectionChangedAction.Add)
            {
                foreach (var item in e.NewItems)
                {
                    await DamageMinion(item as MinionCard);
                }
            }
        }

        private async Task DamageMinion(MinionCard card)
        {
            bool decision = await ConfirmActivateUI.MakeChoice(Card);

            if (decision)
            {
                arrows.RemoveCounters(1);
                card.CharStats.Health.TakeDamage(new(card, 2, card: Card));

                if (arrows.CountersLeft == 0)
                    VillainTurnController.instance.MinionsInPlay.CollectionChanged -= MinionAdded;
            }
        }

        public override void OnExitPlay()
        {
            VillainTurnController.instance.MinionsInPlay.CollectionChanged -= MinionAdded;
        }
    }
}

