using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    Transform targetParent;
    [SerializeField]
    List<CardType> typesToAccept = new List<CardType>();

    public void OnPointerEnter(PointerEventData eventData){}

    public void OnDrop(PointerEventData eventData)
    {
        var draggable = eventData.pointerDrag.GetComponent<Draggable>();
        if (draggable != null)
        {
            if (typesToAccept.Contains(draggable.GetComponent<CardUI>().card.data.cardType))
            {
                draggable.ogParent = targetParent;
                var card = draggable.GetComponent<CardUI>().card;
                if (!card.inPlay)
                {
                    card.inPlay = true;
                    GameObject.Find("PlayerUI").GetComponent<Player>().hand.Remove(card);
                    /*if (card.data.triggerType != null && card.data.triggerType.triggerName == "OnEnterPlay")
                        draggable.gameObject.SendMessage("OnEnterPlay", SendMessageOptions.DontRequireReceiver);*/
                }
                    
            }
            
        }
    }

    public void OnPointerExit(PointerEventData eventData){}
}
