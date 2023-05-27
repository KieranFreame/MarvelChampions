using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : IStat
{
    #region Properties
        public int BaseHP { get; private set; }
        private int _currHealth;
        public int CurrentHealth
        {
            get { return _currHealth; }
            set
            {
                _currHealth = value;
                HealthChanged?.Invoke();
            }
        }
        [SerializeField] private bool _tough = false;
        public bool Tough
        {
            get { return _tough; }
            set
            {
                _tough = value;
                OnToggleTough?.Invoke(_tough);
            }
        }
    #endregion

    public dynamic Owner { get; private set; }

    #region Events
    public event UnityAction Defeated;
    public event UnityAction<bool> OnToggleTough;
    public event UnityAction HealthChanged;
    #endregion

    public Health(Identity owner, AlterEgoData data)
    {
        Owner = owner;
        CurrentHealth = BaseHP = data.baseHP;
    }

    public Health(Villain owner)
    {
        Owner = owner;
        CurrentHealth = BaseHP = owner.BaseHP;


    }
    public Health(Card owner, CardData data)
    {
        Owner = owner;

        if (Owner is MinionCard)
            CurrentHealth = BaseHP = (data as MinionCardData).baseHealth;
        else
            CurrentHealth = BaseHP = (data as AllyCardData).BaseHP;
    }

    public void TakeDamage(int damage)
    {
        if (damage > 0)
        {
            if (Tough)
            {
                Tough = false;
                return;
            }

            CurrentHealth -= damage;

            if (CurrentHealth <= 0)
            {
                CurrentHealth = 0;
                Defeated?.Invoke();
            }
        }
    }
    public void RecoverHealth(int healing)
    {
        CurrentHealth += healing;

        if (CurrentHealth > BaseHP)
            CurrentHealth = BaseHP;
    }
    public void IncreaseMaxHealth(int amount)
    {
        BaseHP += amount;
        RecoverHealth(amount);
    }
    public bool Damaged() => CurrentHealth < BaseHP;
}
