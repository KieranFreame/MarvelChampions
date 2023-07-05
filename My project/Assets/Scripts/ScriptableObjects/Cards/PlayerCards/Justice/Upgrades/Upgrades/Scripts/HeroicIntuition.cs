using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Heroic Intuition", menuName = "MarvelChampions/Card Effects/Justice/Heroic Intuition")]
public class HeroicIntuition : PlayerCardEffect
{
    public override async Task OnEnterPlay()
    {
        _owner.CharStats.Thwarter.CurrentThwart++;
        await Task.Yield();
    }

    public override bool CanBePlayed()
    {
        //There is not already a Heroic Intuition in play
        if (_owner.CardsInPlay.Permanents.Any(x => x.CardName == "Heroic Intuition"))
            return false;

        return base.CanBePlayed();
    }

    public override void OnExitPlay()
    {
        _owner.CharStats.Thwarter.CurrentThwart--;
    }
}
