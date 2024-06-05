using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class IdentityPointer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Identity identity;

    [SerializeField] Transform uiCardChoices;
    [SerializeField] Transform ExhaustParent;
    [SerializeField] Transform ReadyParent;

    public void OnPointerEnter(PointerEventData eventData) 
    {
        if (identity == null)
            identity = GetComponent<Player>().Identity;

        uiCardChoices.SetParent(identity.Exhausted ? ExhaustParent : ReadyParent, false);
        uiCardChoices.localPosition = Vector3.zero;
        uiCardChoices.gameObject.SetActive(true); 
    }
    
    
    public void OnPointerExit(PointerEventData eventData) => uiCardChoices.gameObject.SetActive(false);
    
}
