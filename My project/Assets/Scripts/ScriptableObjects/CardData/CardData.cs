using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CardData : ScriptableObject
{
    [Header ("Fields")]
    public string cardName;
    [TextArea(3, 5)]
    public string cardDescription;
    public List<string> cardTags = new List<string>();
    public List<string> keywords = new List<string>();
    public List<ActionData> actions = new List<ActionData>();
    
    public bool faceUp;

    [Header ("Sprites")]
    public Sprite cardBack;
    public Sprite cardFace;

    public CardType cardType;
    public string abilityLoader;
}
