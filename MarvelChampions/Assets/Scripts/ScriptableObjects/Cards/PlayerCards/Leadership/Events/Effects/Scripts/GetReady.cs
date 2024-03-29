using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Get Ready", menuName = "MarvelChampions/Card Effects/Leadership/Get Ready")]
public class GetReady : PlayerCardEffect
{
    readonly List<AllyCard> allies = new();

    public override bool CanBePlayed()
    {
        if (base.CanBePlayed())
        {
            allies.Clear();
            allies.AddRange(_owner.CardsInPlay.Allies);
            allies.RemoveAll(x => !x.Exhausted);

            return allies.Count > 0;
        }
        
        return false;
    }

    public override async Task OnEnterPlay()
    {
        AllyCard target = await TargetSystem.instance.SelectTarget(allies);

        if (target != null)
            target.Ready();
    }
}
