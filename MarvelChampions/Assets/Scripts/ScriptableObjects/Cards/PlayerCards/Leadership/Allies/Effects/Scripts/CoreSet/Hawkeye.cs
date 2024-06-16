using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace CoreSet
{
    [CreateAssetMenu(fileName = "Hawkeye (Clint Barton)", menuName = "MarvelChampions/Card Effects/Leadership/Hawkeye (Clint Barton)")]
    public class Hawkeye : PlayerCardEffect, IOptional
    {
        MinionCard target;
        Counters arrows;

        public override async Task OnEnterPlay()
        {
            arrows = _card.gameObject.AddComponent<Counters>();
            arrows.AddCounters(4);

            RevealEncounterCardSystem.OnEncounterCardRevealed += CanRespond;

            await Task.Yield();
        }

        private void CanRespond(EncounterCard arg0)
        {
            if (arg0.CardType == CardType.Minion)
            {
                target = (MinionCard)arg0;
                EffectManager.Inst.Responding.Add(this);
            }   
        }

        public override Task Resolve()
        {
            arrows.RemoveCounters(1);
            target.CharStats.Health.TakeDamage(new(target, 2, card: Card));

            if (arrows.CountersLeft == 0)
                RevealEncounterCardSystem.OnEncounterCardRevealed -= CanRespond;

            return Task.CompletedTask;
        }

        public override void OnExitPlay()
        {
            RevealEncounterCardSystem.OnEncounterCardRevealed -= CanRespond;
        }
    }
}

