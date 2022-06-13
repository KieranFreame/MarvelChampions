using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Transform ogParent;

    private void Start()
    {
        ogParent = transform.parent;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        ogParent = this.transform.parent;
        this.transform.SetParent(GameObject.Find("Canvas").transform);
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        this.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        this.transform.SetParent(ogParent);
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
}
