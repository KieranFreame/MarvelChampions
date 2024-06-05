using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using TMPro;
using UnityEditor.Search;
using UnityEngine;

public class CardSearchPanel : MonoBehaviour
{
    private static CardSearchPanel instance;

    TMP_InputField cardInputField;

    [SerializeField] Transform contentTransform;
    [SerializeField] GameObject contentPrefab;

    public static Transform ContentTransform { get => instance.contentTransform; }

    private void Awake()
    {
        instance ??= this;

        foreach (CardData c in Database.Instance.cards.database)
        {
            if (c is not PlayerCardData) continue;
            if ((c as PlayerCardData).cardAspect == Aspect.Hero || (c as PlayerCardData).cardAspect == Aspect.Campaign) continue;

            CreateSearchEntry(c);
        }

        AspectFilter(0);
    }

    void SearchByName(string searchQuery)
    {
        PlayerCardData data;

        if (searchQuery == string.Empty)
        {
            foreach (Transform child in contentTransform)
            {
                data = child.GetComponent<CardPoolPanel>().card;

                if (DeckPreviewPanel.cardTabs.ContainsKey(data))
                    if (DeckPreviewPanel.cardTabs[data].GetComponent<CardPoolPanel>().count >= data.maxCopies)
                        continue;

                if (data.cardAspect != Aspect.Basic)
                    if (DeckPreviewPanel.chosenAspect != data.cardAspect)
                        continue;

                child.gameObject.SetActive(true);
            }
        }
        else
        {
            foreach (Transform child in contentTransform)
            {
                data = child.GetComponent<CardPoolPanel>().card;
                child.gameObject.SetActive(data.cardName.ToLower().Contains(searchQuery.ToLower()));
            }
        }
        
    }

    public void AspectFilter(int filter)
    {
        Aspect aspect = (Aspect)filter;

        foreach (CardDisplayUI child in contentTransform.GetComponentsInChildren<CardDisplayUI>(true))
        {
            child.gameObject.SetActive(child.CardData.cardAspect == aspect);
        }
    }

    void CreateSearchEntry(CardData c)
    {
        GameObject card = Instantiate(PrefabFactory.instance.GetDisplayPrefab(c), contentTransform);
        card.GetComponent<CardDisplayUI>().CardData = c as PlayerCardData;
    }
}
