using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnClick : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    Transform uiCardChoices;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!(PayCostSystem.instance.stateMachine.currentState == PayCostSystem.instance.states[0]) || !(TargetSystem.instance.stateMachine.currentState == TargetSystem.instance.states[0]))
            return;

        if (uiCardChoices.gameObject.activeSelf)
        {
            uiCardChoices.gameObject.SetActive(false);
            return;
        }

        uiCardChoices.gameObject.SetActive(true);
    }
}
