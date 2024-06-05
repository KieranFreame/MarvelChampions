using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScenarioSettings : MonoBehaviour
{
    [SerializeField] Image menuVillainPortrait;
    [SerializeField] Image customizeVillainPortrait;

    [SerializeField] TMP_Dropdown difficultyPanel;
    [SerializeField] ModularSetPanel modularsPanel;

    [SerializeField] TMP_Text briefingText;

    ScenarioData sData;

    private void OnEnable()
    {
        difficultyPanel.onValueChanged.AddListener(ChangeDifficulty);
        ChangeDifficulty(difficultyPanel.value);
    }

    private void ChangeDifficulty(int difficulty)
    {
        ScenarioManager.inst.Difficulty = (Difficulty)difficulty;
        modularsPanel.AddModular(((Difficulty)difficulty).ToString());
    }

    public void ChangeScenario(string scenarioName)
    {
        ScenarioData data = Resources.Load<ScenarioData>("Scenarios/" + scenarioName);

        modularsPanel.AddModular(data);

        ScenarioManager.inst.villain = data.villain;
        ScenarioManager.inst.EncounterDeck = new(TextReader.PopulateDeck(data.villain.deckPath + ".txt"));

        briefingText.text = $"Name: {data.villain.villainName}\n\n{data.Briefing}";

        customizeVillainPortrait.sprite = menuVillainPortrait.sprite = data.villain.villainArt;
        menuVillainPortrait.transform.Find("PreviewText").gameObject.SetActive(false);
        customizeVillainPortrait.transform.Find("Placeholder").gameObject.SetActive(false);
    }
    
}
