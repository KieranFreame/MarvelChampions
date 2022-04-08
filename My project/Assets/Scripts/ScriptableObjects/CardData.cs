using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CardData : ScriptableObject
{
    [SerializeField]
    public string cardName;
    [TextArea(3, 5)]
    public string cardDescription;
    [SerializeField]
    public List<string> cardTags = new List<string>();
    [SerializeField]
    public string aspect;
    [SerializeField]
    public bool faceUp;
}
