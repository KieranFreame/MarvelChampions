using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "HeroSet", fileName = "New Hero Set")]
public class HeroSetData : ScriptableObject
{
    public List<PlayerCard> heroSet = new List<PlayerCard>();
    public List<CardData> nemesisSet = new List<CardData>();
    public List<CardData> obligation = new List<CardData>();
}
