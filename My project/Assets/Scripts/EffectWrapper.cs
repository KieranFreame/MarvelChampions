using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectWrapper
{
    public PlayerCardEffect effect { get; private set; }

    public EffectWrapper (PlayerCardEffect _effect)
    {
        effect = _effect;
    }
}
