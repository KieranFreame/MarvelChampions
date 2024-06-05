using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardPoolPanel : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] TMP_Text cardCount;
    [SerializeField] TMP_Text cardName;
    public PlayerCardData card { get; set; }
    public int count { get; set; } = 0;

    private void Awake()
    {
        cardName = transform.Find("CardName").GetComponent<TMP_Text>();
        transform.Find("CardCount")?.TryGetComponent(out cardCount);
    }

    public void Increment()
    {
        count++;
        cardCount.text = "x" + count;
    }
    public void Decrement()
    {
        count--;
        cardCount.text = "x" + count;
    }

    public void CustomizePanel()
    {
        cardName.text = card.cardName;

        Image image = GetComponent<Image>();

        switch (card.cardAspect)
        {
            case Aspect.Aggression:
                image.color = new Color32(253, 87, 102, 255);
                break;
            case Aspect.Justice:
                image.color = new Color32(216, 229, 87, 255);
                break;
            case Aspect.Leadership:
                image.color = new Color32(87, 213, 229, 255);
                break;
            case Aspect.Protection:
                image.color = new Color32(109, 200, 120, 255);
                break;
            default:
                image.color = new Color32(145, 145, 145, 255);
                break;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (card.cardAspect != Aspect.Hero)
            DeckPreviewPanel.playerDeck.Remove(card);
    }
}
