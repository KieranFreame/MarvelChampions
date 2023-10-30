using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using TMPro;
using UnityEngine;

public class DeckPreviewPanel : MonoBehaviour
{
    [Header("Hero Panel")]
    [SerializeField] TMP_Dropdown heroDropdown;

    [Header("Content")]
    [SerializeField] Transform contentTransform;
    [SerializeField] GameObject contentPrefab;
    [SerializeField] GameObject removableContentPrefab;

    public static ObservableCollection<PlayerCardData> playerDeck = new();
    public static Dictionary<PlayerCardData, GameObject> cardTabs = new();

    public static ObservableCollection<Aspect> chosenAspects = new();
    public static int maximumAspects { get; private set; }

    private void Awake()
    {
        heroDropdown.onValueChanged.AddListener(AddHeroCards);
        playerDeck.CollectionChanged += AdjustTabs;
    }

    void Clear()
    {
        playerDeck.Clear();
        chosenAspects.Clear();
        maximumAspects = 1;
    }

    private void AddHeroCards(int value)
    {
        Clear();

        if (value == 0)
        {
            return;
        }

        string identityName = heroDropdown.captionText.text;

        string[] identity = identityName.Split('(');
        identity[1] = identity[1].Trim(')');
        identity[1] = string.Concat(identity[1].Where(c => !char.IsWhiteSpace(c)));

        //identity[0] = Hero Name, identity[1] = Alter-Ego Name

        foreach (CardData c in TextReader.PopulateDeck(identity[1] + ".txt"))
        {
            if (c.cardID.Contains("-O-")) continue; //obligation
            playerDeck.Add(c as PlayerCardData);
        }

        switch (identity[0]) //Hero Name
        {
            case "Spider-Woman":
                maximumAspects = 2; //Double Agent, gets access to two aspects
                break;
            case "Adam Warlock": //Battle Mage, gets access to four aspects
                maximumAspects = 4;
                break;
            default:
                break;
        }    
    }

    void AdjustTabs(object sender, NotifyCollectionChangedEventArgs e)
    {
        PlayerCardData data;
        CardPoolPanel panel = null;

        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
                data = (e.NewItems[0] as PlayerCardData);

                if (cardTabs.ContainsKey(data))
                {
                    panel = cardTabs[data].GetComponent<CardPoolPanel>();
                    panel.Increment();
                }
                else
                {
                    GameObject tab = Instantiate(data.cardAspect == Aspect.Hero ? contentPrefab : removableContentPrefab, contentTransform);
                    cardTabs.Add(data, tab);

                    tab.transform.Find("CardName").GetComponent<TMP_Text>().text = data.cardName;
                    tab.GetComponent<CardPoolPanel>().card = data;
                    tab.GetComponent<CardPoolPanel>().Increment();

                    if (data.cardAspect != Aspect.Basic && data.cardAspect != Aspect.Hero)
                        chosenAspects.Add(data.cardAspect);
                }
                break;
            case NotifyCollectionChangedAction.Remove:
                data = (e.OldItems[0] as PlayerCardData);

                panel = cardTabs[data].GetComponent<CardPoolPanel>();
                panel.Decrement();

                if (panel.count <= 0)
                {
                    cardTabs.Remove(data);
                    Destroy(panel.gameObject);
                }

                if (playerDeck.Where(x => x.cardAspect != Aspect.Basic && x.cardAspect != Aspect.Hero).Count() == 0)
                    chosenAspects.Remove(data.cardAspect);

                break;
        }
    }
}
