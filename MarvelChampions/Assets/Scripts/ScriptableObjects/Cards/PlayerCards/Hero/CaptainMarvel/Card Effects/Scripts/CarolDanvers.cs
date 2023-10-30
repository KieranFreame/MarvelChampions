using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Carol Danvers", menuName = "MarvelChampions/Identity Effects/Captain Marvel/AlterEgo")]
public class CarolDanvers : IdentityEffect
{
    public override void LoadEffect(Player _owner)
    {
        owner = _owner;
        hasActivated = false;

        TurnManager.OnStartPlayerPhase += Reset;
    }

    public override bool CanActivate()
    {
        return !hasActivated;
    }

    public override void Activate()
    {
        DrawCardSystem.Instance.DrawCards(new(1));
        hasActivated = true;
    }
}
