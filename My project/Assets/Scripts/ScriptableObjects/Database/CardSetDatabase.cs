using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Card Set Database", menuName = "MarvelChampions/Databases/Card Set Database")]
public class CardSetDatabase : ScriptableObject
{
    public List<CardSet> database;
}
