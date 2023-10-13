using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class Defender
{
    #region Properties
    private int _defence;
    public int CurrentDefence
    {
        get => _defence;
        set
        {
            _defence = value;
            DefenceChanged?.Invoke();
        }
    }
    public int BaseDEF { get; private set; }
    #endregion

    private readonly Identity _owner;

    #region Events
    public event UnityAction DefenceChanged;
    #endregion

    #region Delegates
    public delegate Task<int> DefenceModifier(int defence);
    public List<DefenceModifier> modifiers { get; private set; } = new();
    #endregion

    public Defender(Identity owner, HeroData data)
    {
        _owner = owner;
        CurrentDefence = BaseDEF = data.baseDEF;
    }

    public async Task<int> Defend()
    {
        _owner.Exhaust();

        int DEF = _defence;

        for (int i = modifiers.Count - 1; i >= 0; i--)
        {
            DEF = await modifiers[i](DEF);
            if (DEF < 0) DEF = 0;
        }

        return DEF;
    }
}
