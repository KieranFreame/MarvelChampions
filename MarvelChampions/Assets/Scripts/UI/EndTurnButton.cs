using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTurnButton : MonoBehaviour
{
    private GameObject endTurnBtn;

    private void Awake()
    {
        endTurnBtn = transform.GetChild(0).gameObject;
    }

    private void OnEnable()
    {
        TurnManager.OnEndPlayerPhase += ToggleEndTurnBtn;
        TurnManager.OnEndVillainPhase += ToggleEndTurnBtn;
    }

    private void OnDisable()
    {
        TurnManager.OnEndPlayerPhase -= ToggleEndTurnBtn;
        TurnManager.OnEndVillainPhase -= ToggleEndTurnBtn;
    }

    private void ToggleEndTurnBtn() => endTurnBtn.SetActive(TurnManager.IsPlayerPhase);
}
