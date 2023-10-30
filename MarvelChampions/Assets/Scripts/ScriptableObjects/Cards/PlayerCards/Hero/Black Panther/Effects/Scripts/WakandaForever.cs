using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Wakanda Forever", menuName = "MarvelChampions/Card Effects/Black Panther/Wakanda Forever")]
public class WakandaForever : PlayerCardEffect
{
    public override bool CanBePlayed()
    {
        if (base.CanBePlayed())
        {
            if (_owner.Identity.ActiveIdentity is not Hero)
                return false;

            if (_owner.CardsInPlay.Permanents.Any(x => x.Data.cardTraits.Contains("Black Panther")) == false)
                return false;

            return true;
        }

        return false;
    }

    public override async Task OnEnterPlay()
    {
        List<PlayerCard> pantherUpgrades = _owner.CardsInPlay.Permanents.Where(x => x.Data.cardTraits.Contains("Black Panther")).ToList();

        CancellationToken token = FinishButton.ToggleFinishButton(true, FinishedSelecting);
        List<PlayerCard> effectOrder = await TargetSystem.instance.SelectTargets(pantherUpgrades, pantherUpgrades.Count, token);
        FinishButton.ToggleFinishButton(false, FinishedSelecting);

        for (int i = 0; i < effectOrder.Count; i++)
        {
            Debug.Log("Activating " + effectOrder[i].CardName);
            await (effectOrder[i].Effect as IBlackPanther).Special(i == pantherUpgrades.Count - 1);
        }
    }

    private void FinishedSelecting()
    {
        FinishButton.ToggleFinishButton(false, FinishedSelecting);
    }
}
