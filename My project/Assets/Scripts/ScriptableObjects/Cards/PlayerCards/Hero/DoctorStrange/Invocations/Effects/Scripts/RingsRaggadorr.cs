using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Rings of Raggadorr", menuName = "MarvelChampions/Card Effects/Doctor Strange/Invocations/Rings of Raggadorr")]
public class RingsRaggadorr : PlayerCardEffect, IInvocation
{
    List<ICharacter> friendlies;

    public override bool CanActivate()
    {
        if (_owner == null)
            _owner = TurnManager.Players.Single(x => x.Identity.Hero.Name == "Doctor Strange");

        friendlies = new() { _owner };
        friendlies.AddRange(_owner.CardsInPlay.Allies);
        friendlies.RemoveAll(x => x.CharStats.Health.Tough);

        return friendlies.Count > 0;
    }

    public async Task Special()
    {
        CancellationToken token = FinishButton.ToggleFinishButton(true, FinishedSelection);
        List<ICharacter> targets = await TargetSystem.instance.SelectTargets(friendlies, 3, token);
        FinishButton.ToggleFinishButton(false, FinishedSelection);

        foreach (var target in targets)
        {
            target.CharStats.Health.Tough = true;
        }
    }

    private void FinishedSelection()
    {
        FinishButton.ToggleFinishButton(false, FinishedSelection);
    }
}
