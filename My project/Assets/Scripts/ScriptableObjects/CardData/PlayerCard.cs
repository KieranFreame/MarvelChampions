using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player Card", menuName = "Card/Player Card")]
public class PlayerCard : CardData
{
    public int cardCost;
    public string aspect;
    public List<Resource> resources;
    public bool exhausted;

    [Header("ResourceLoader")]
    public Resource actionResource;

    public virtual List<Resource> GetResource(PlayerCard cardToPlay)
    {
        return resources;
    }
}
