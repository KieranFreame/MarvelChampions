using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace CoreSet
{
    [CreateAssetMenu(fileName = "Black Widow", menuName = "MarvelChampions/Card Effects/Protection/Allies/Black Widow")]
    public class BlackWidow : PlayerCardEffect, IOptional
    {
        /// <summary>
        /// When an encounter card is revealed, exhaust Black Widow and spend a scientific resource; cancel its effects and discard it. Surge.
        /// </summary>

        public override Task OnEnterPlay()
        {
            EffectManager.Inst.OnEffectActivated += CanRespond;
            return Task.CompletedTask;
        }

        public void CanRespond(ICard card)
        {
            if (_card.Exhausted || !_owner.HaveResource(Resource.Scientific))
                return;

            EffectManager.Inst.Responding.Add(this);
        }

        public override async Task Resolve()
        {
            await PayCostSystem.instance.GetResources(Resource.Scientific, 1);
            _card.Exhaust();
            CancelEffect();
            RevealEncounterCardSystem.Instance.CancelCard();

            ScenarioManager.inst.Surge(_owner);
        }

        public void CancelEffect()
        {
            Stack<IEffect> stack = new Stack<IEffect>();

            while (EffectManager.Inst.Resolving.Peek().Card is not EncounterCard && EffectManager.Inst.Resolving.Count != 0)
            {
                stack.Push(EffectManager.Inst.Resolving.Pop());
            }

            EffectManager.Inst.Resolving.Pop(); //Cancel effect

            while (stack.Count > 0)
            {
                EffectManager.Inst.Resolving.Push(stack.Pop());
            }
        }

        public override void OnExitPlay()
        {
            EffectManager.Inst.OnEffectActivated -= CanRespond;
        }
    }
}

