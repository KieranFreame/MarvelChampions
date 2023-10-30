using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnClick : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    Transform uiCardChoices;

    private void OnEnable()
    {
        UIManager.instance.OnUIEnable += DisableChoices;
    }

    private void OnDisable()
    {
        UIManager.instance.OnUIEnable -= DisableChoices;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (uiCardChoices.gameObject.activeSelf)
        {
            uiCardChoices.gameObject.SetActive(false);
            return;
        }

        UIManager.instance.UIEnable();
        uiCardChoices.gameObject.SetActive(true);
    }

    private void DisableChoices()
    {
        uiCardChoices.gameObject.SetActive(false);
    }
}
