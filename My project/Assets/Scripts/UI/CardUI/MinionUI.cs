using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MinionUI : EncounterCardUI
{
    private MinionCard minionCard;
    [SerializeField] private TMP_Text minionScheme;
    [SerializeField] private TMP_Text minionAttack;
    [SerializeField] private TMP_Text minionHealth;

    Attacker _attacker;
    Schemer _schemer;
    Health _health;

    #region Colours
    protected Color32 _atk = new(255, 0, 0, 255);
    protected Color32 _stun = new(21, 214, 47, 255);
    protected Color32 _sch = new(0, 65, 255, 255);
    protected Color32 _confuse = new(116, 11, 154, 255);
    protected Color32 _hp = new(226, 154, 27, 255);
    protected Color32 _tough = new(255, 113, 0, 255);
    #endregion

    protected override void OnEnable()
    {
        base.OnEnable();

        if (minionCard == null)
        {
            TryGetComponent(out minionCard);
            minionCard.SetupComplete += LoadData;
            return;
        }

        _attacker.OnToggleStun += ToggleStun;
        _attacker.AttackChanged += AttackChanged;
        _schemer.OnToggleConfuse += ToggleConfuse;
        _health.OnToggleTough += ToggleTough;
        _health.HealthChanged += HealthChanged;
    }
    protected void OnDisable()
    {
        _attacker.OnToggleStun -= ToggleStun;
        _attacker.AttackChanged -= AttackChanged;
        _schemer.OnToggleConfuse -= ToggleConfuse;
        _health.OnToggleTough -= ToggleTough;
        _health.HealthChanged -= HealthChanged;
    }

    protected override void LoadData()
    {
        base.LoadData();

        _attacker = minionCard.CharStats.Attacker;
        _schemer = minionCard.CharStats.Schemer;
        _health = minionCard.CharStats.Health;

        _attacker.OnToggleStun += ToggleStun;
        _attacker.AttackChanged += AttackChanged;

        _schemer.OnToggleConfuse += ToggleConfuse;

        _health.OnToggleTough += ToggleTough;
        _health.HealthChanged += HealthChanged;

        minionScheme.text = minionCard.BaseScheme.ToString();
        minionAttack.text = minionCard.BaseAttack.ToString();
        minionHealth.text = minionCard.BaseHP.ToString();

        minionCard.SetupComplete -= LoadData;
    }

    private void ToggleStun(bool stunned)
    {
        minionAttack.text = stunned ? "S" : _attacker.CurrentAttack.ToString();
        minionAttack.GetComponentInParent<Image>().color = stunned ? _stun : _atk;
    }

    private void ToggleConfuse(bool confused)
    {
        minionScheme.text = confused ? "C" : _schemer.CurrentScheme.ToString();
        minionScheme.GetComponentInParent<Image>().color = confused ? _confuse : _sch;
    }
    private void ToggleTough(bool tough)
    {
        minionHealth.text = tough ? "T" : _health.CurrentHealth.ToString();
        minionHealth.GetComponentInParent<Image>().color = tough ? _tough : _hp;
    }

    private void AttackChanged()
    {
        minionAttack.text = _attacker.Stunned ? "S" : _attacker.CurrentAttack.ToString();
        minionAttack.GetComponentInParent<Image>().color = _attacker.Stunned ? _stun : _atk;
    }

    private void HealthChanged()
    {
        minionHealth.text = _health.Tough ? "T" : _health.CurrentHealth.ToString();
    }
}
