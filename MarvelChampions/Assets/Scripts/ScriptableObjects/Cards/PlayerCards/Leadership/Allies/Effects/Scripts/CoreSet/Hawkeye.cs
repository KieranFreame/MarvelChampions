using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
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
            arrows = _card.gameObject.AddComponent<Counters>();
            arrows.AddCounters(4);

            VillainTurnController.instance.MinionsInPlay.CollectionChanged += MinionAdded;

            await Task.Yield();
        }

        private void MinionAdded(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action is NotifyCollectionChangedAction.Add)
            {
                EffectResolutionManager.Instance.ResolvingEffects.Push(this);
            }
        }

        public override Task Resolve()
        {
            MinionCard card = VillainTurnController.instance.MinionsInPlay.Last();

            arrows.RemoveCounters(1);
            card.CharStats.Health.TakeDamage(new(card, 2, card: Card));

            if (arrows.CountersLeft == 0)
                VillainTurnController.instance.MinionsInPlay.CollectionChanged -= MinionAdded;

            return Task.CompletedTask;
        }

        public override void OnExitPlay()
        {
            VillainTurnController.instance.MinionsInPlay.CollectionChanged -= MinionAdded;
        }
    }
}

