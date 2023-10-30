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

        cardInputField = GetComponentInChildren<TMP_InputField>();
        cardInputField.onValueChanged.AddListener(SearchByName);

        DeckPreviewPanel.chosenAspects.CollectionChanged += FilterByAspect;

        foreach (CardData c in Database.Instance.cards.database)
        {
            if (c is not PlayerCardData) continue;
            if ((c as PlayerCardData).cardAspect == Aspect.Hero || (c as PlayerCardData).cardAspect == Aspect.Campaign) continue;

            CreateSearchEntry(c);
        }
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
                    if (!DeckPreviewPanel.chosenAspects.Contains(data.cardAspect))
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

    void FilterByAspect(object sender, NotifyCollectionChangedEventArgs e)
    {
        PlayerCardData data;

        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
                if (DeckPreviewPanel.chosenAspects.Count >= DeckPreviewPanel.maximumAspects)
                {
                    foreach (Transform child in contentTransform)
                    {
                        data = child.GetComponent<CardPoolPanel>().card;

                        if(DeckPreviewPanel.cardTabs.ContainsKey(data))
                            if (DeckPreviewPanel.cardTabs[data].GetComponent<CardPoolPanel>().count >= data.maxCopies)
                                continue;

                        if (child.gameObject.activeSelf)
                            child.gameObject.SetActive(e.NewItems.Contains(data.cardAspect) && data.cardAspect != Aspect.Basic);
                    }
                }
                break;
            case NotifyCollectionChangedAction.Remove:
                if (DeckPreviewPanel.chosenAspects.Count < DeckPreviewPanel.maximumAspects)
                {
                    foreach (Transform child in contentTransform)
                    {
                        data = child.GetComponent<CardPoolPanel>().card;

                        if (DeckPreviewPanel.cardTabs.ContainsKey(data))
                            if (DeckPreviewPanel.cardTabs[data].GetComponent<CardPoolPanel>().count >= data.maxCopies)
                                continue;

                        child.gameObject.SetActive(true);
                    }
                }
                break;
        }
    }

    void CreateSearchEntry(CardData c)
    {
        GameObject card = Instantiate(contentPrefab, contentTransform);
        card.GetComponent<CardPoolPanel>().card = c as PlayerCardData;
        card.transform.Find("CardName").GetComponent<TMP_Text>().text = c.cardName;
    }
}
