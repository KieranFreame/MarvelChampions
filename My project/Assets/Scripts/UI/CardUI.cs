using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardUI : MonoBehaviour
{
    public Card card;

    [Header("UI Elements")]
    public Text cardNameText;
    public Text cardDescText;
    public Text cardTagsText;

    protected virtual void Start()
    {
        if (card.data != null)
            StartCoroutine(LoadData());
    }

    protected virtual IEnumerator LoadData()
    {
        yield return new WaitUntil(() => card != null);

        cardNameText.text = card.data.cardName;
        cardDescText.text = card.data.cardDescription;

        if (cardTagsText != null)
        {
            for (int i = 0; i < card.data.cardTags.Count - 1; i++)
            {
                cardTagsText.text += $"{card.data.cardTags[i]}, ";
            }
            cardTagsText.text += card.data.cardTags[card.data.cardTags.Count - 1];
        }

        Refresh();
    }

    public virtual void Refresh()
    {
        cardNameText.text = card.data.cardName;
        cardDescText.text = card.data.cardDescription;

        if (cardTagsText != null)
        {
            cardTagsText.text = string.Empty;

            for (int i = 0; i < card.data.cardTags.Count - 1; i++)
            {
                cardTagsText.text += $"{card.data.cardTags[i]}, ";
            }
            cardTagsText.text += card.data.cardTags[card.data.cardTags.Count - 1];
        }
    }

    public T GetCard<T>() where T : CardData
    {
        return (card.data as T);
    }
}
