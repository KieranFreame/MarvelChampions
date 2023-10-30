using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Internal;

[CreateAssetMenu(fileName = "New Player Card", menuName = "MarvelChampions/Card/Player Card")]
public class PlayerCardData : CardData
{
    [Header("Player Card")]
    public int cardCost;
    public int maxCopies = 3;
    public bool isUnique;
    public Aspect cardAspect;
    public List<Resource> cardResources;
    public PlayerCardEffect effect;
}
