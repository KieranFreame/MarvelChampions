using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VillainUI : MonoBehaviour
{
    private VillainData data;

    [Header("UI Elements")]
    public Image villainProfile;
    public Text villainStage;
    public Text villainScheme;
    public Text villainAttack;

    public Text villainHealth;

    private void Start()
    {
        data = GetComponent<Villain>().data;

        SetStage();

        villainScheme.text = data.stages[GetComponent<Villain>().stage - 1].baseScheme.ToString();
        villainAttack.text = data.stages[GetComponent<Villain>().stage - 1].baseAttack.ToString();
        villainHealth.text = data.stages[GetComponent<Villain>().stage - 1].baseHitpoints.ToString();
    }

    public void Refresh()
    {
        SetStage();

        villainScheme.text = GetComponent<Schemer>()._scheme.ToString();
        villainAttack.text = GetComponent<Attacker>()._attack.ToString();
        villainHealth.text = GetComponent<Health>().currHealth.ToString();
    }

    private void SetStage()
    {
        switch (GetComponent<Villain>().stage)
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
    }
}
