using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Effect Database", menuName = "MarvelChampions/Databases/Effect Database")]
public class EffectDatabase : ScriptableObject
{
    public List<CardEffect> database = new List<CardEffect>();
}
