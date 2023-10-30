using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CardData : ScriptableObject
{
    [Header("Card Data")]
    public string cardID;
    public string cardName;
    [TextArea (5, 10)]
    public string cardDesc;
    public CardType cardType;
    public List<string> cardTraits;
    public Sprite cardArt;
}
