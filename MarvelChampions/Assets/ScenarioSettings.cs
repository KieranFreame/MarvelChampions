using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScenarioSettings : MonoBehaviour
{
    Transform difficultyPanel;
    Transform scenarioPanel;
    Transform modularsPanel;

    private void Awake()
    {
        difficultyPanel = transform.Find("DifficultySelectPanel");
        scenarioPanel = transform.Find("ScenarioSelectPanel");
        modularsPanel = transform.Find("ModularSetPanel");
    }

    private void OnEnable()
    {
        difficultyPanel.GetComponentInChildren<TMP_Dropdown>().onValueChanged.AddListener(ChangeDifficulty);
        scenarioPanel.GetComponentInChildren<TMP_Dropdown>().onValueChanged.AddListener(ChangeScenario);
    }

    private void Start()
    {
        ScenarioManager.inst.Difficulty = (Difficulty)difficultyPanel.GetComponentInChildren<TMP_Dropdown>().value;
    }

    private void ChangeDifficulty(int difficulty) => ScenarioManager.inst.Difficulty = (Difficulty)difficulty;

    private void ChangeScenario(int value)
    {
        if (value == 0)
        {
            ScenarioManager.inst.scenarioData = null;
            return;
        }

        string scenarioName = scenarioPanel.GetComponentInChildren<TMP_Dropdown>().captionText.text;
        ScenarioData data = Resources.Load<ScenarioData>("Scenarios/" + scenarioName);

        modularsPanel.GetComponent<ModularSetPanel>().AddModular(data);

        ScenarioManager.inst.scenarioData = data;
    }
    
}
