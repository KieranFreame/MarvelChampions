using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace CoreSet
{
    [CreateAssetMenu(fileName = "Black Widow", menuName = "MarvelChampions/Card Effects/Protection/Allies/Black Widow")]
    public class BlackWidow : PlayerCardEffect
    {
        public override Task OnEnterPlay()
        {
            RevealEncounterCardSystem.Instance.EffectCancelers.Add(CancelEffect);
            return Task.CompletedTask;
        }

        public async Task<bool> CancelEffect(ICard cardToCancel)
        {
            if (!_owner.Hand.cards.Any(x => x.Resources.Contains(Resource.Scientific) || x.Resources.Contains(Resource.Wild)))
                return false;

            var card = cardToCancel as EncounterCard;

            bool decision = await ConfirmActivateUI.MakeChoice(Card);

            if (decision)
            {
                await PayCostSystem.instance.GetResources(Resource.Scientific, 1);
                Card.Exhaust();
                ScenarioManager.inst.Surge(_owner);

                ScenarioManager.inst.EncounterDeck.Discard(card);

                return true;
            }

            return false;
        }

        public override void OnExitPlay()
        {
            RevealEncounterCardSystem.Instance.EffectCancelers.Remove(CancelEffect);
        }
    }
}

