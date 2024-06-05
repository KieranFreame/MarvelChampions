using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeckPreviewPanel : MonoBehaviour
{
    public static DeckPreviewPanel instance;

    [Header("Main Menu")]
    [SerializeField] Image heroPortrait;
    [SerializeField] Image aspectBackground;
    [SerializeField] TMP_Text aspectText;

    [Header("Hero Panel")]
    [SerializeField] TMP_Dropdown heroDropdown;
    [SerializeField] TMP_Text deckCount;

    [Header("Content")]
    [SerializeField] Transform contentTransform;
    [SerializeField] GameObject contentPrefab;

    public static ObservableCollection<PlayerCardData> playerDeck = new();
    public static Dictionary<PlayerCardData, GameObject> cardTabs = new();

    public static Aspect chosenAspect = Aspect.Campaign;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        playerDeck.CollectionChanged += AdjustTabs;
    }

    void Clear()
    {
        for (int i = playerDeck.Count  - 1; i >= 0; i--)
            playerDeck.RemoveAt(i);

        chosenAspect = Aspect.Campaign;
        heroPortrait.sprite = null;

        
        PlayerData.Instance.HeroData = null;
        PlayerData.Instance.AlterEgoData = null;

        GameObject.Find("HeroPanel").GetComponent<HeroPanel>().UpdateIdentity(null);
    }

    public void AddHeroCards(string alterEgoName)
    {
        for (int i = playerDeck.Count - 1; i >= 0; i--)
            if (playerDeck[i].cardAspect == Aspect.Hero)
                playerDeck.RemoveAt(i);


        alterEgoName = string.Concat(alterEgoName.Where(c => !char.IsWhiteSpace(c)));

        foreach (CardData c in TextReader.PopulateDeck(alterEgoName + ".txt"))
        {
            if (c.cardID.Contains("-O-")) { ScenarioManager.inst.obligations.Add(c as EncounterCardData); continue; }

            playerDeck.Add(c as PlayerCardData);
        }

        heroPortrait.sprite = PlayerData.Instance.HeroData.heroArt;
        heroPortrait.transform.Find("PreviewText").gameObject.SetActive(false);

        GameObject.Find("HeroPanel").GetComponent<HeroPanel>().UpdateIdentity(PlayerData.Instance.HeroData);
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
                    GameObject tab = Instantiate(contentPrefab, contentTransform);
                    cardTabs.Add(data, tab);

                    tab.GetComponent<CardPoolPanel>().card = data;
                    tab.GetComponent<CardPoolPanel>().CustomizePanel();

                    tab.GetComponent<CardPoolPanel>().Increment();

                    if (chosenAspect != data.cardAspect)
                        if (data.cardAspect != Aspect.Basic && data.cardAspect != Aspect.Hero)
                        {
                            chosenAspect = data.cardAspect;
                            aspectText.text = data.cardAspect.ToString();

                            aspectBackground.color = data.cardAspect switch
                            {
                                Aspect.Leadership => Color.blue,
                                Aspect.Aggression => Color.red,
                                Aspect.Justice => Color.yellow,
                                _ => Color.green, //Protection
                            };
                        }
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
                {
                    chosenAspect = Aspect.Campaign;
                    aspectText.text = "";
                    aspectBackground.color = Color.white;
                }
                    

                break;
        }

        deckCount.text = $"YOUR DECK ({playerDeck.Count}/50)";
    }
}
