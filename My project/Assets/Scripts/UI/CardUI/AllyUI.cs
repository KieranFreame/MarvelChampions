using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AllyUI : PlayerCardUI
{
    [Header("UI Components")]
    [SerializeField] private Text cardThwText;
    [SerializeField] private Text cardAtkText;
    [SerializeField] private Text cardHPText;

    #region Components
    AllyCard allyCard;
    #endregion
    #region Colours
    protected Color32 _atk = new (255, 0, 0, 255);
    protected Color32 _stun = new (21, 214, 47, 255);
    protected Color32 _thw = new (0, 65, 255, 255);
    protected Color32 _confuse = new (116, 11, 154, 255);
    protected Color32 _hp = new (226, 154, 27, 255);
    protected Color32 _tough = new (255, 113, 0, 255);
    #endregion

    protected override void OnEnable()
    {
        base.OnEnable();

        if (allyCard == null)
        {
            TryGetComponent(out allyCard);
            return;
        }
            

        allyCard.CharStats.Attacker.OnToggleStun += ToggleStun;
        allyCard.CharStats.Attacker.AttackChanged += AttackChanged;

        allyCard.CharStats.Thwarter.OnToggleConfuse += ToggleConfuse;
        allyCard.CharStats.Thwarter.ThwartChanged += ThwartChanged;

        allyCard.CharStats.Health.OnToggleTough += ToggleTough;
        allyCard.CharStats.Health.HealthChanged += HealthChanged;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        allyCard.CharStats.Attacker.OnToggleStun -= ToggleStun;
        allyCard.CharStats.Attacker.AttackChanged -= AttackChanged;

        allyCard.CharStats.Thwarter.OnToggleConfuse -= ToggleConfuse;
        allyCard.CharStats.Thwarter.ThwartChanged -= ThwartChanged;

        allyCard.CharStats.Health.OnToggleTough -= ToggleTough;
        allyCard.CharStats.Health.HealthChanged -= HealthChanged;
    }

    protected override void LoadData()
    {
        cardThwText.text = allyCard.CharStats.Thwarter.BaseThwart.ToString();
        cardAtkText.text = allyCard.CharStats.Attacker.BaseATK.ToString();
        cardHPText.text = allyCard.CharStats.Health.BaseHP.ToString();

        allyCard.CharStats.Attacker.OnToggleStun += ToggleStun;
        allyCard.CharStats.Attacker.AttackChanged += AttackChanged;

        allyCard.CharStats.Thwarter.OnToggleConfuse += ToggleConfuse;
        allyCard.CharStats.Thwarter.ThwartChanged += ThwartChanged;

        allyCard.CharStats.Health.OnToggleTough += ToggleTough;
        allyCard.CharStats.Health.HealthChanged += HealthChanged;

        base.LoadData();
    }
    #region Stat Funcs
    private void AttackChanged() { cardAtkText.text = allyCard.CharStats.Attacker.CurrentAttack.ToString(); }
    private void ThwartChanged() { cardThwText.text = allyCard.CharStats.Thwarter.CurrentThwart.ToString(); }
    private void HealthChanged() { cardHPText.text = allyCard.CharStats.Health.CurrentHealth.ToString(); }
    #endregion
    #region Status Funcs
    private void ToggleStun(bool stunned)
    {
        cardAtkText.text = stunned ? "S" : allyCard.CharStats.Attacker.CurrentAttack.ToString();
        cardAtkText.GetComponentInParent<Image>().color = stunned ? _stun : _atk;
    }
    private void ToggleConfuse(bool confused)
    {
        cardThwText.text = confused ? "C" : allyCard.CharStats.Thwarter.CurrentThwart.ToString();
        cardThwText.GetComponentInParent<Image>().color = confused ? _confuse : _thw;
    }
    private void ToggleTough(bool tough)
    {
        cardHPText.text = tough ? "T" : allyCard.CharStats.Health.CurrentHealth.ToString();
        cardHPText.GetComponentInParent<Image>().color = tough ? _hp : _tough;
    }
    #endregion
}
