using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SchemeUI : EncounterCardUI
{
    SchemeCard scheme;
    Threat _threat;

    [Header("Scheme UI")]
    [SerializeField] private Image CardIcon;
    [SerializeField] private Text currentThreatText;

    [Header("Main Scheme UI")]
    [SerializeField] private Text maxThreatText;
    [SerializeField] private Text _accelerationText;

    protected override void OnEnable()
    {
        base.OnEnable();

        if (scheme == null)
        {
            TryGetComponent(out scheme);

            if (scheme == null)
                transform.parent.TryGetComponent(out scheme);

            scheme.SetupComplete += LoadData;
            return;
        }

        _threat.ThreatChanged += UpdateThreat;

        if (scheme is MainSchemeCard)
        {
            _threat.MaxThreatChanged += UpdateMaxThreat;
            _threat.AccelerationChanged += UpdateAcceleration;
        }
    }

    private void OnDisable()
    {
        _threat.ThreatChanged -= UpdateThreat; 
        _threat.MaxThreatChanged -= UpdateMaxThreat;
        _threat.AccelerationChanged -= UpdateAcceleration;
    }

    protected override void LoadData()
    {
        base.LoadData();

        _threat = scheme.Threat;
        _threat.ThreatChanged += UpdateThreat;

        if (scheme is MainSchemeCard)
        {
            _threat.MaxThreatChanged += UpdateMaxThreat;
            _threat.AccelerationChanged += UpdateAcceleration;
        }

        currentThreatText.text = scheme.StartingThreat.ToString();

        if (scheme is MainSchemeCard)
        {
            maxThreatText.text = (scheme as MainSchemeCard).MaximumThreat.ToString();
            _accelerationText.text = (scheme as MainSchemeCard).Acceleration.ToString();
        }

        scheme.SetupComplete -= LoadData;
    }

    private void UpdateThreat(int newThreat)
    {
        currentThreatText.text = newThreat.ToString();
    }

    private void UpdateAcceleration(int newAccel) => _accelerationText.text = newAccel.ToString();
    private void UpdateMaxThreat(int newMaxThreat) => maxThreatText.text = newMaxThreat.ToString();
    
}
