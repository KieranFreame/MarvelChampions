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
    public event UnityAction OnTakeDamage;
    #endregion

    public List<IModifyDamage> Modifiers { get; private set; } = new();

    public Health(Identity owner, AlterEgoData data)
    {
        Owner = owner;
        CurrentHealth = BaseHP = data.baseHP;
    }

    public Health(Villain owner)
    {
        Owner = owner;
        CurrentHealth = BaseHP = owner.BaseHP;
        owner.StageAdvanced += AdvanceStage;

    }
    public Health(ICard owner, CardData data)
    {
        Owner = owner;

        if (Owner is MinionCard)
            CurrentHealth = BaseHP = (data as MinionCardData).baseHealth;
        else
            CurrentHealth = BaseHP = (data as AllyCardData).BaseHP;
    }

    public async void TakeDamage(int damage)
    {
        if (damage > 0)
        {
            if (Tough)
            {
                Tough = false;
                return;
            }

            for (int i = Modifiers.Count - 1; i >= 0; i--)
            {
                damage = await Modifiers[i].OnTakeDamage(damage);

                if (damage < 0)
                {
                    damage = 0;
                    break;
                } 
            }

            CurrentHealth -= damage;

            if (CurrentHealth <= 0)
            {
                CurrentHealth = 0;
                Defeated?.Invoke();
                return;
            }

            OnTakeDamage?.Invoke();
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
    private void AdvanceStage(int amount) => CurrentHealth = BaseHP = Owner.BaseHP;
    public bool Damaged() => CurrentHealth < BaseHP;
}
