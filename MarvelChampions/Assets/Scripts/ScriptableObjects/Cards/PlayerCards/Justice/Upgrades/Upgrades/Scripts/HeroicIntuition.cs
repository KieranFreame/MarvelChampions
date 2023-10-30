using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Heroic Intuition", menuName = "MarvelChampions/Card Effects/Justice/Heroic Intuition")]
public class HeroicIntuition : PlayerCardEffect
{
    public override Task OnEnterPlay()
    {
        _owner.CharStats.Thwarter.CurrentThwart++;
        return Task.CompletedTask;
    }

    public override bool CanBePlayed()
    {
        if (base.CanBePlayed())
        {
            return !_owner.CardsInPlay.Permanents.Any(x => x.CardName == "Heroic Intuition");
        }
        
        return false;
    }

    public override void OnExitPlay()
    {
        _owner.CharStats.Thwarter.CurrentThwart--;
    }
}
