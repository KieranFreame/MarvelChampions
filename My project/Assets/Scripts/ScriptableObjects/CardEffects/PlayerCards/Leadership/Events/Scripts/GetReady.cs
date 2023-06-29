using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Get Ready", menuName = "MarvelChampions/Card Effects/Leadership/Get Ready")]
public class GetReady : PlayerCardEffect
{
    List<AllyCard> allies = new();

    public override void OnDrawn(Player player, PlayerCard card)
    {
        _owner = player;
        Card = card;
    }

    public override bool CanBePlayed()
    {
        allies.AddRange(_owner.CardsInPlay.Allies);
        allies.RemoveAll(x => !x.Exhausted);

        return allies.Count > 0;
    }

    public override async Task OnEnterPlay(Player player, PlayerCard card)
    {
        AllyCard target = await TargetSystem.instance.SelectTarget(allies);

        if (target != null)
            target.Ready();
    }
}
