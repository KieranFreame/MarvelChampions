using System.Collections;
using System.Collections.Generic;
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

            RevealEncounterCardSystem.OnEncounterCardRevealed += EncounterCardRevealed;

            await Task.Yield();
        }

        private async void EncounterCardRevealed(EncounterCard card)
        {
            if (card is not MinionCard) return;

            MinionCard minion = card as MinionCard;

            bool decision = await ConfirmActivateUI.MakeChoice(Card);

            if (decision)
            {
                arrows.RemoveCounters(1);
                minion.CharStats.Health.TakeDamage(2);

                if (arrows.CountersLeft == 0)
                    RevealEncounterCardSystem.OnEncounterCardRevealed -= EncounterCardRevealed;
            }
        }

        public override void OnExitPlay()
        {
            RevealEncounterCardSystem.OnEncounterCardRevealed -= EncounterCardRevealed;
        }
    }
}

