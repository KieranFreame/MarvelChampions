using System.Collections.Generic;
using System.Threading.Tasks;

public class EffectResolutionManager
{
    private static EffectResolutionManager instance;
    public Stack<IEffect> ResolvingEffects = new();

    public static EffectResolutionManager Instance
    {
        get
        {
            if (instance == null)
                instance = new EffectResolutionManager();

            return instance;
        }
    }

    public async Task ResolveEffects()
    {
        while (ResolvingEffects.Count > 0)
        {
            var effect = ResolvingEffects.Pop();

            if (effect is PlayerCardEffect)
            {
                if (effect is IOptional)
                {
                    if (!await ConfirmActivateUI.MakeChoice(effect.Card))
                    {
                        continue;
                    }
                }
            }

            await effect.Resolve();
        }
    }
}

