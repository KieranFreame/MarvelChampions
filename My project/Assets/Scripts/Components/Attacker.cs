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
    public ICharacter Owner { get; private set; }
    public List<Keywords> Keywords { get; private set; } = new();
    #region Events
    public event UnityAction<bool> OnToggleStun;
    public event UnityAction AttackChanged;
    #endregion

    #region Constructors
    public Attacker(ICard owner, CardData data)
    {
        Owner = owner as ICharacter;

        if (Owner is MinionCard)
            CurrentAttack = BaseATK = (data as MinionCardData).baseAttack;
        else
            CurrentAttack = BaseATK = (data as AllyCardData).BaseATK;
    }
    public Attacker(Player owner, HeroData data)
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

    public AttackAction Attack(AttackAction action = null)
    {
        if (Owner is IExhaust && action == null)
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

        return action ?? new AttackAction(CurrentAttack, _keywords:Keywords, owner:Owner);
    }

    private void AdvanceStage(int newStage) => CurrentAttack += (Owner as Villain).BaseAttack - BaseATK;
}
