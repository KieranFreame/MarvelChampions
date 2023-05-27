using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VillainUI : MonoBehaviour
{
    private Villain villain;

    [Header("UI Elements")]
    [SerializeField] private Image villainProfile;
    [SerializeField] private Text villainStage;
    [SerializeField] private Text villainScheme;
    [SerializeField] private Text villainAttack;
    [SerializeField] private Text villainHealth;

    #region Colours
    protected Color32 _atk = new(255, 0, 0, 255);
    protected Color32 _stun = new(21, 214, 47, 255);
    protected Color32 _thw = new(0, 65, 255, 255);
    protected Color32 _confuse = new(116, 11, 154, 255);
    protected Color32 _hp = new(255, 255, 255, 255);
    protected Color32 _tough = new(255, 113, 0, 255);
    #endregion

    #region Components
    Attacker _attacker;
    Schemer _schemer;
    Health _health;
    #endregion

    private void OnEnable()
    {
        if (villain == null) {
            villain = GetComponent<Villain>();
            villain.SetupComplete += SetUI;
            return;
        }

        _attacker ??= villain.CharStats.Attacker;
        _schemer ??= villain.CharStats.Schemer;
        _health ??= villain.CharStats.Health;

        _attacker.OnToggleStun += ToggleStun;
        _attacker.AttackChanged += AttackChanged;

        _schemer.OnToggleConfuse += ToggleConfuse;
        _schemer.SchemeChanged += SchemeChanged;

        _health.OnToggleTough += ToggleTough;
        _health.HealthChanged += HealthChanged;
    }
    private void OnDisable()
    {
        _attacker.OnToggleStun -= ToggleStun;
        _attacker.AttackChanged -= AttackChanged;

        _schemer.OnToggleConfuse -= ToggleConfuse;
        _schemer.SchemeChanged -= SchemeChanged;

        _health.OnToggleTough -= ToggleTough;
        _health.HealthChanged -= HealthChanged;
    }

    #region Setup
    private void SetUI()
    {
        switch (GetComponent<Villain>().Stage)
        {
            case 1:
                villainStage.text = "I";
                break;
            case 2:
                villainStage.text = "II";
                break;
            case 3:
                villainStage.text = "III";
                break;
        }

        villainScheme.text = villain.BaseScheme.ToString();
        villainAttack.text = villain.BaseAttack.ToString();
        villainHealth.text = villain.BaseHP.ToString();

        _attacker ??= villain.CharStats.Attacker;
        _schemer ??= villain.CharStats.Schemer;
        _health ??= villain.CharStats.Health;

        _attacker.OnToggleStun += ToggleStun;
        _attacker.AttackChanged += AttackChanged;

        _schemer.OnToggleConfuse += ToggleConfuse;
        _schemer.SchemeChanged += SchemeChanged;

        _health.OnToggleTough += ToggleTough;
        _health.HealthChanged += HealthChanged;

        villain.SetupComplete -= SetUI;
    }

    #endregion

    #region Stat Funcs
    private void AttackChanged() { villainAttack.text = villain.CharStats.Attacker.Stunned ? "S" : villain.CharStats.Attacker.CurrentAttack.ToString(); }
    private void SchemeChanged() { villainScheme.text = villain.CharStats.Schemer.Confused ? "C" : villain.CharStats.Schemer.CurrentScheme.ToString(); }
    private void HealthChanged() { villainHealth.text = villain.CharStats.Health.Tough ? "T" : villain.CharStats.Health.CurrentHealth.ToString(); }
    #endregion

    #region Status Funcs
    private void ToggleStun(bool stunned)
    {
        villainAttack.text = stunned ? "S" : _attacker.CurrentAttack.ToString();
        villainAttack.GetComponentInParent<Image>().color = stunned ? _stun : _atk;
    }
    private void ToggleConfuse(bool confused)
    {
        villainScheme.text = confused ? "C" : _schemer.CurrentScheme.ToString();
        villainScheme.GetComponentInParent<Image>().color = confused ? _confuse : _thw;
    }
    private void ToggleTough(bool tough)
    {
        villainHealth.text = tough ? "T" : _health.CurrentHealth.ToString();
        villainHealth.GetComponentInParent<Image>().color = tough ? _tough : _hp;
    }
    #endregion
}
