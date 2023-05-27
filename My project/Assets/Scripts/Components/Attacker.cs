using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Attacker : IStat
{
    #region Properties
    private int _currAttack;
    public int CurrentAttack {
        get => _currAttack;
        set
        {
            _currAttack = value;
            AttackChanged?.Invoke();
        }
    }
    public int BaseATK { get; private set; }

    private bool stunned = false;
    public bool Stunned
    {
        get { return stunned; }
        set
        {
            stunned = value;
            OnToggleStun?.Invoke(stunned);
        }
    }
    #endregion
    public dynamic Owner { get; private set; }
    #region Events
    public event UnityAction<bool> OnToggleStun;
    public event UnityAction AttackChanged;
    #endregion

    #region Constructors
    public Attacker(Card owner, CardData data)
    {
        Owner = owner;

        if (Owner is MinionCard)
            CurrentAttack = BaseATK = (data as MinionCardData).baseAttack;
        else
            CurrentAttack = BaseATK = (data as AllyCardData).BaseATK;
    }
    public Attacker(Identity owner, HeroData data)
    {
        Owner = owner;
        CurrentAttack = BaseATK = data.baseATK;
    }
    public Attacker(Villain owner)
    {
        Owner = owner;
        owner.StageAdvanced += AdvanceStage;
        CurrentAttack = BaseATK = owner.BaseAttack;
    }
    #endregion

    public AttackAction Attack()
    {
        if (Owner is IExhaust)
        {
            if ((Owner as IExhaust).Exhausted)
                return null;

            (Owner as IExhaust).Exhaust();
        }

        if (Stunned)
        {
            Stunned = false;
            return null;
        }

        var attack = new AttackAction(CurrentAttack, _keywords:new List<Keywords>(), owner:Owner);
        return attack;
    }

    private void AdvanceStage(int newStage) => BaseATK = Owner.BaseAttack;
}
