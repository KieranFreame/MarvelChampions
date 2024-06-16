using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.Events;

public class EffectManager
{
    private static EffectManager instance;
    public Stack<IEffect> Resolving = new();
    public List<IEffect> Responding = new();

    public static EffectManager Inst
    {
        get
        {
            instance ??= new();
            return instance;
        }
    }

    public UnityAction<ICard> OnEffectActivated;

    public async Task AddEffect(ICard card, IEffect effect)
    {
        Resolving.Push(effect);
        OnEffectActivated?.Invoke(card);
        await CheckResponding();
    }

    public async Task CheckResponding()
    {
        if (Responding.Count > 0)
        {
            List<IEffect> mandatoryEffects = new(Responding.Where(x => x is not IOptional));
            Responding.RemoveAll(x => x is not IOptional);

            foreach (var item in mandatoryEffects)
                Resolving.Push(item);

            for (int i = Responding.Count - 1; i >= 0; i--)
            {
                if (i >= Responding.Count)
                    i = Responding.Count - 1;

                var respond = Responding[i];

                if (await ConfirmActivateUI.MakeChoice(respond.Card))
                {
                    if (!respond.Card.InPlay)
                        await PlayCardSystem.Instance.InitiatePlayCard(new(respond.Card as PlayerCard));

                    Resolving.Push(respond);
                }
            }
        }

        Responding.Clear();
        await ResolveEffects();
    }

    private async Task ResolveEffects()
    {
        while (Resolving.Count != 0)
        {
            await Resolving.Pop().Resolve();
        }
    }
}

