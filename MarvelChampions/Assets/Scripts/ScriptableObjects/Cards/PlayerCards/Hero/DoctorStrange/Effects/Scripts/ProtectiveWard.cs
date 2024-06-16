using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Protective Ward", menuName = "MarvelChampions/Card Effects/Doctor Strange/Protective Ward")]
public class ProtectiveWard : PlayerCardEffect
{
    public override void OnDrawn() => EffectManager.Inst.OnEffectActivated += CanRespond;

    public void CanRespond(ICard card)
    {
        if (card.CardType != CardType.Treachery || _owner.Identity.ActiveIdentity is not Hero || _owner.ResourcesAvailable(_card) < _card.CardCost) return;

        EffectManager.Inst.Responding.Add(this);
    }

    public override Task Resolve()
    {
        Stack<IEffect> stack = new Stack<IEffect>();

        while (EffectManager.Inst.Resolving.Peek().Card.CardType != (CardType)7 && EffectManager.Inst.Resolving.Count != 0)
        {
            stack.Push(EffectManager.Inst.Resolving.Pop());
        }

        EffectManager.Inst.Resolving.Pop(); //Cancel effect

        while (stack.Count > 0)
        {
            EffectManager.Inst.Resolving.Push(stack.Pop());
        }

        EffectManager.Inst.OnEffectActivated -= CanRespond;
        return Task.CompletedTask;
    }

    public override void OnDiscard() =>  EffectManager.Inst.OnEffectActivated -= CanRespond;
    
}
