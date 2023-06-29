using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player Card", menuName = "MarvelChampions/Card/Player Card")]
public class PlayerCardData : CardData
{
    [Header("Player Card")]
    public int cardCost;
    public bool isUnique;
    public Aspect cardAspect;
    public List<Resource> cardResources;
    public PlayerCardEffect effect;
}
