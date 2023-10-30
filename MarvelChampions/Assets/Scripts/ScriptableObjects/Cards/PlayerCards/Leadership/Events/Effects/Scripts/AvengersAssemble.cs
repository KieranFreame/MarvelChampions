using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Avengers Assemble", menuName = "MarvelChampions/Card Effects/Leadership/Avengers Assemble")]
public class AvengersAssemble : PlayerCardEffect
{
    List<AllyCard> avengers;

    public override bool CanBePlayed()
    {
        if (base.CanBePlayed())
        {
            avengers = _owner.CardsInPlay.Allies.Where(x => x.CardTraits.Contains("Avenger")).ToList();

            if (avengers.Count == 0) return false;
            if (avengers.Where(x => x.Exhausted).Count() == 0) return false;

            return true;
        }

        return false;
    }
    public override Task OnEnterPlay()
    {
        if (_owner.Identity.IdentityTraits.Contains("Avenger"))
        {
            _owner.Ready();
            _owner.CharStats.Attacker.CurrentAttack++;
            _owner.CharStats.Thwarter.CurrentThwart++;
        }

        foreach (var a in avengers)
        {
            a.Ready();
            a.CharStats.Attacker.CurrentAttack++;
            a.CharStats.Thwarter.CurrentThwart++;
        }

        TurnManager.OnEndPlayerPhase += EndOfPhase;

        return Task.CompletedTask;
    }

    private void EndOfPhase()
    {
        if (_owner.Identity.IdentityTraits.Contains("Avenger"))
        {
           _owner.CharStats.Attacker.CurrentAttack--;
           _owner.CharStats.Thwarter.CurrentThwart--;
        }

        avengers.RemoveAll(x => x == null);

        foreach (var a in avengers)
        {
            a.CharStats.Attacker.CurrentAttack--;
            a.CharStats.Thwarter.CurrentThwart--;
        }
    }
}
