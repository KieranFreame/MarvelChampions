using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "I'm Tough", menuName = "MarvelChampions/Card Effects/Rhino/Im Tough")]
public class ImTough : CardEffect
{
    /// <summary>
    /// Give Rhino a tough status card. If Rhino already has a tough status card, this card gains surge.
    /// </summary>
    public override void OnEnterPlay(Villain owner, Card card)
    {
        if (owner.CharStats.Health.Tough)
        {
            owner.Surge(FindObjectOfType<Player>());
            return;
        }

        owner.GetComponent<Health>().Tough = true;
    }
}
