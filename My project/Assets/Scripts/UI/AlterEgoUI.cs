using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlterEgoUI : MonoBehaviour
{
    #region UIElements
    private Identity owner;
    public Image alteregoPortrait;
    public Text recovery;
    public Text health;
    #endregion

    #region Components
    Health _health;
    Recovery _recovery;
    #endregion

    private void OnEnable()
    {
        if (owner is null) return;

        _health.HealthChanged += HealthChanged;
        _health.OnToggleTough += ToggleTough;
        _recovery.RecoveryChanged += RecoveryChanged;
    }

    private void OnDisable()
    {
        _health.HealthChanged -= HealthChanged;
        _recovery.RecoveryChanged -= RecoveryChanged;
    }

    public void LoadUI(Identity _owner)
    {
        owner = _owner;

        _health = owner.CharStats.Health;
        _recovery = owner.CharStats.Recovery;

        _health.HealthChanged += HealthChanged;
        _health.OnToggleTough += ToggleTough;
        _recovery.RecoveryChanged += RecoveryChanged;

        //alteregoPortrait.sprite = alterego.art;
        health.text = owner.BaseHP.ToString();
    }

    private void ToggleTough(bool tough)
    {
        health.text = tough ? "T" : _health.CurrentHealth.ToString();
        health.GetComponentInParent<Image>().color = tough ? new Color32(0, 0, 100, 100) : new Color32(255, 113, 0, 255); //white : orange
    }

    private void HealthChanged()
    {
        health.text = _health.Tough ? "T" : _health.CurrentHealth.ToString();
    }

    private void RecoveryChanged()
    {
        recovery.text = _recovery.REC.ToString();
    }
}
