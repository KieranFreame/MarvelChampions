using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Villain", menuName = "Villain")]
public class VillainData : ScriptableObject
{
    public string villainName;
    public string deckPath;
    public Sprite villainArt;
    public List<VillainStage> stages = new();
}
