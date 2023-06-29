using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroUI : MonoBehaviour
{
    #region UIElements
    public Image heroPortrait;
    public Text heroATK;
    public Text heroTHW;
    public Text heroDEF;
    public Text heroHP;
    #endregion

    #region Components
    Attacker _attacker;
    Thwarter _thwarter;
    Defender _defender;
    Health _health;
    Sprite _art;
    #endregion

    #region Colours
    protected Color32 _atk = new (255, 0, 0, 255);
    protected Color32 _stun = new(21, 214, 47, 255);
    protected Color32 _thw = new (0, 65, 255, 255);
    protected Color32 _confuse = new(116, 11, 154, 255);
    protected Color32 _hp = new (255, 255, 255, 255);
    protected Color32 _tough = new (255, 113, 0, 255);
    #endregion

    private void OnEnable()
    {
        heroPortrait.sprite = _art;

        _attacker.AttackChanged += AttackChanged;
        _health.HealthChanged += HealthChanged;
        _thwarter.ThwartChanged += ThwartChanged;
        _defender.DefenceChanged += DefenceChanged;

        _attacker.OnToggleStun += ToggleStun;
        _thwarter.OnToggleConfuse += ToggleConfuse;
        _health.OnToggleTough += ToggleTough;
    }

    private void OnDisable()
    {
        _attacker.AttackChanged -= AttackChanged;
        _health.HealthChanged -= HealthChanged;
        _thwarter.ThwartChanged -= ThwartChanged;
        _defender.DefenceChanged -= DefenceChanged;

        _attacker.OnToggleStun -= ToggleStun;
        _thwarter.OnToggleConfuse -= ToggleConfuse;
        _health.OnToggleTough -= ToggleTough;
    }

    public void LoadUI(Player owner)
    {
        _attacker = owner.CharStats.Attacker;
        _thwarter = owner.CharStats.Thwarter;
        _defender = owner.CharStats.Defender;
        _health = owner.CharStats.Health;

        _art = owner.Identity.Hero.Art;

        _attacker.AttackChanged += AttackChanged;
        _health.HealthChanged += HealthChanged;
        _thwarter.ThwartChanged += ThwartChanged;
        _defender.DefenceChanged += DefenceChanged;

        _attacker.OnToggleStun += ToggleStun;
        _thwarter.OnToggleConfuse += ToggleConfuse;
        _health.OnToggleTough += ToggleTough;

        AttackChanged();
        ThwartChanged();
        DefenceChanged();
        HealthChanged();
    }

    private void AttackChanged() { heroATK.text = _attacker.CurrentAttack.ToString(); }
    private void ThwartChanged() { heroTHW.text = _thwarter.CurrentThwart.ToString(); }
    private void DefenceChanged() { heroDEF.text = _defender.CurrentDefence.ToString(); }
    private void HealthChanged() { heroHP.text = _health.Tough ? "T" : _health.CurrentHealth.ToString(); }

    private void ToggleStun(bool stunned)
    {
        heroATK.text = stunned ? "S" : _attacker.CurrentAttack.ToString();
        heroATK.GetComponentInParent<Image>().color = stunned ? _stun : _atk;
    }
    private void ToggleConfuse(bool confused)
    {
        heroTHW.text = confused ? "C" : _thwarter.CurrentThwart.ToString();
        heroTHW.GetComponentInParent<Image>().color = confused ? _confuse : _thw;
    }
    private void ToggleTough(bool tough)
    {
        heroHP.text = tough ? "T" : _health.CurrentHealth.ToString();
        heroHP.GetComponentInParent<Image>().color = tough ? _tough : _hp;
    }
}
