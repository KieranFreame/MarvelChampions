using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;


public abstract class Card : MonoBehaviour
{
    public CardData Data { get; private set; }
    public GameObject Owner { get; private set; } //player or villain
    public CardEffect Effect { get; protected set; }
    public Zone CurrZone { get; set; }
    public Zone PrevZone { get; set; }
    public bool InPlay { get; set; } = false;
    public bool FaceUp { get; set; }

    [SerializeField] private GameObject cardBack;

    public event UnityAction SetupComplete;
    public void EnterPlay()
    {
        dynamic o = Owner.GetComponent<Player>() != null ? Owner.GetComponent<Player>() : Owner.GetComponent<Villain>();
        Effect.OnEnterPlay(o, this);
    }

    private void OnMouseEnter()
    {
        UIManager.ShowCardInfo(this);
    }
    private void OnMouseExit()
    {
        UIManager.ShowCardInfo();
    }

    public virtual void LoadCardData(CardData _data, GameObject owner)
    {
        Owner = owner;
        Data = _data;
        GetComponent<CardUI>().CardArt = Data.cardArt;

        CardEffect e = Database.GetCardEffectById(Data.cardID);

        if (e != null)
            Effect = e;

        SetupComplete?.Invoke();
    }

    public virtual void Flip()
    {
        FaceUp = !FaceUp;
        cardBack.SetActive(FaceUp);
    }

    #region Properties
    public string CardName
    {
        get => Data.cardName;
    }
    public string CardDesc
    {
        get => Data.cardDesc;
    }
    public List<string> CardTraits
    {
        get => Data.cardTraits;
    }
    public CardType CardType
    {
        get => Data.cardType;
    }
    #endregion
}
