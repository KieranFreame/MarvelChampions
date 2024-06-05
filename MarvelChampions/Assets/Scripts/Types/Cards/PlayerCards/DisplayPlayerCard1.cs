using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class DisplayPlayerCard : MonoBehaviour
{
    public PlayerCardData Data { get; set; }

    public virtual void LoadCardData(PlayerCardData data)
    {

        Data = data;
        GetComponent<CardUI>().CardArt = Data.cardArt;

        foreach (string trait in Data.cardTraits)
            CardTraits.Add(trait);
    }


    //public PlayerCardEffect Effect { get; private set; }
    public virtual List<Resource> Resources { get => Data.cardResources; }
    public int CardCost{ get => Data.cardCost; }
    public Aspect CardAspect { get => Data.cardAspect; }
    public string CardName { get => Data.cardName; }
    public string CardDesc { get => Data.cardDesc; }
    public CardType CardType { get => Data.cardType; }
    public List<string> CardTraits { get => Data.cardTraits; }
}
