using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Winds of Watoomb", menuName = "MarvelChampions/Card Effects/Doctor Strange/Invocations/Winds of Watoomb")]
public class WindsWatoomb : PlayerCardEffect, IInvocation
{
    public override bool CanActivate()
    {
        return true;
    }

    public Task Special()
    {
        DrawCardSystem.Instance.DrawCards(new(3, _owner));
        return Task.CompletedTask;
    }
}
