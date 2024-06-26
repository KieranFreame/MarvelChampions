using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Legal Practice", menuName = "MarvelChampions/Card Effects/She-Hulk/Legal Practice")]
public class LegalPractice : PlayerCardEffect
{
    public override bool CanBePlayed()
    {
        if (base.CanBePlayed())
        {
            return _owner.Identity.ActiveIdentity is AlterEgo;
        }

        return false;
    }

    public override async Task OnEnterPlay()
    {
        if (_owner.Hand.cards.Count == 0)
        {
            await _owner.CharStats.InitiateThwart(new(0, Owner));
            return;
        }

        CancellationToken token = FinishButton.ToggleFinishButton(true, CardsSelected);

        List<PlayerCard> discards = await TargetSystem.instance.SelectTargets(_owner.Hand.cards.ToList(), 5, token);

        FinishButton.ToggleFinishButton(false, CardsSelected);

        int thwart = discards.Count;

        foreach (PlayerCard c in discards)
        {
            _owner.Hand.Discard(c);
        }
        
        await _owner.CharStats.InitiateThwart(new(thwart, Owner));
    }

    private void CardsSelected()
    {
        FinishButton.ToggleFinishButton(false, CardsSelected);
    }
}
