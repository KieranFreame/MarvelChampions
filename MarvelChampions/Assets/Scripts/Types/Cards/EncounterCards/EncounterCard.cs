using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Events;

public class EncounterCard : MonoBehaviour, ICard
{
    public Villain Owner { get; private set; }
    public int BoostIcons { get; set; }
    public EncounterCardData Data { get; set; }
    public Zone CurrZone { get; set; }
    public Zone PrevZone { get; set; }
    public bool InPlay { get; set; }
    public bool FaceUp { get; set; }
    public string CardName { get => Data.cardName; }
    public string CardDesc { get => Data.cardDesc; }
    public CardType CardType { get => Data.cardType; }
    public ObservableSet<string> CardTraits { get; protected set; } = new();

    public event UnityAction SetupComplete;

    protected EncounterCardEffect effect;
    protected EncounterCardEffect boost;

    public async Task OnRevealCard()
    {
        await effect.WhenRevealed(Owner, this, FindObjectOfType<Player>());
    }

    public virtual void LoadCardData(EncounterCardData data, Villain owner)
    {
        //EncounterCard
        Owner = owner;
        BoostIcons = data.boostIcons;

        Data = data;

        foreach (string trait in Data.cardTraits)
            CardTraits.AddItem(trait);

        TryGetComponent(out CardUI cardUI);

        if (cardUI == null)
            cardUI = GetComponentInChildren<CardUI>();

        cardUI.CardArt = Data.cardArt;

        if (Data.effect != null)
        {
            effect = Instantiate(Data.effect);
            effect.Owner = Owner;
            effect._card = this;
        }
        
        if (Data.boost != null)
        {
            boost = Instantiate(Data.boost);
            boost.Owner = Owner;
            boost._card = this;
        }
            

        SetupComplete?.Invoke();
    }

    public virtual EncounterCardEffect Effect
    {
        get => effect;
    }
    
    public virtual EncounterCardEffect Boost
    {
        get => boost;
    }
}
