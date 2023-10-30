using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card Database", menuName = "MarvelChampions/Databases/Card Database")]
public class CardDatabase : ScriptableObject
{
    public List<CardData> database = new List<CardData> ();
}
