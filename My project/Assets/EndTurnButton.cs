using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTurnButton : MonoBehaviour
{
    private GameObject endTurnBtn;

    private void OnEnable()
    {
        TurnManager.OnEndPlayerPhase += ToggleEndTurnBtn;
        TurnManager.OnEndVillainPhase += ToggleEndTurnBtn;

        endTurnBtn = transform.Find("EndTurnButton").gameObject;
    }

    private void ToggleEndTurnBtn() => endTurnBtn.SetActive(TurnManager.IsPlayerPhase);
}
