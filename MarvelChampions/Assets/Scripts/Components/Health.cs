using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class Health
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
    public ICharacter Owner { get; private set; }
    #region Events
    public event UnityAction<bool> OnToggleTough;
    public event UnityAction HealthChanged;
    public event UnityAction<DamageAction> OnTakeDamage;
    #endregion

    public delegate Task<DamageAction> ModifyDamage(DamageAction action);
    public List<ModifyDamage> Modifiers { get; private set; } = new();

    public delegate Task WhenDefeated();
    public List<WhenDefeated> Defeated { get; private set; } = new();

    public Health(Player owner, AlterEgoData data)
    {
        Owner = owner;
        CurrentHealth = BaseHP = data.baseHP;
    }

    public Health(Villain owner)
    {
        Owner = owner;
        CurrentHealth = BaseHP = owner.BaseHP;
        owner.Stages.StageAdvanced += AdvanceStage;

    }
    public Health(ICard owner, CardData data)
    {
        Owner = owner as ICharacter;

        if (Owner is MinionCard)
            CurrentHealth = BaseHP = (data as MinionCardData).baseHealth;
        else
            CurrentHealth = BaseHP = (data as AllyCardData).BaseHP;
    }

    public async void TakeDamage(DamageAction action)
    {
        if (action.Value > 0)
        {
            if (Tough)
            {
                Tough = false;
                action.Value = 0;
            }

            for (int i = Modifiers.Count - 1; i >= 0; i--)
            {
                action = await Modifiers[i](action);
            }
        }

        if (action.Value < 0)
            action.Value = 0;

        CurrentHealth -= action.Value;

        if (CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            
            for (int i = Defeated.Count -1; i >= 0; i--)
            {
                await Defeated[i]();
            }

            if (CurrentHealth == 0)
            {
                (Owner as MonoBehaviour).SendMessage("WhenDefeated", SendMessageOptions.DontRequireReceiver);
                return;
            } 
        }

        OnTakeDamage?.Invoke(action);
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

    private void AdvanceStage() => CurrentHealth = BaseHP = (Owner as Villain).BaseHP;
    public bool Damaged() => CurrentHealth < BaseHP;
}
