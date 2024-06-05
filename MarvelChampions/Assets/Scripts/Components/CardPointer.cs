using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class CardPointer : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
{
    private CardData cardData;

    [SerializeField]
    Transform uiCardChoices;

    private void OnDestroy()
    {
        uiCardChoices.gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (cardData == null)
        {
            ICard card = GetComponent<ICard>();

            if (card is PlayerCard)
                cardData = (card as PlayerCard).Data;
            else //encounter card
                cardData = (card as EncounterCard).Data;
        }

        uiCardChoices.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        uiCardChoices.gameObject.SetActive(false);
    }
}
