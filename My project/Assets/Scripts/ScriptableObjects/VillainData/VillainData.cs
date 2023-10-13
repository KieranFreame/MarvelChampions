using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Villain", menuName = "Villain")]
public class VillainData : ScriptableObject
{
    public string villainName;
    public List<string> villainTraits;
    public string deckPath;
    public Sprite villainArt;

    public VillainEffect villainEffect;
    public List<VillainStage> stages = new();
}
