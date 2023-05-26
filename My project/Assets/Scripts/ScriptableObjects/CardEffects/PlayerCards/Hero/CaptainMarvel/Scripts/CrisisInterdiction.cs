using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "CrisisInterdiction", menuName = "MarvelChampions/Card Effects/Captain Marvel/Crisis Interdiction")]
public class CrisisInterdiction : CardEffect
{
    public override void OnEnterPlay(Player owner, Card card)
    {
        var action = new ThwartAction(2);
        ThwartSystem.InitiateThwart(action);

        if (owner.Identity.IdentityTraits.Contains("Aerial".ToLower()))
        {
            ThwartSystem.OnThwartComplete += SecondThwart;
        }
    }

    public void SecondThwart()
    {
        var action = new ThwartAction(2, requirement:"norepeat");
        ThwartSystem.InitiateThwart(action);
        ThwartSystem.OnThwartComplete -= SecondThwart;
    }
}
