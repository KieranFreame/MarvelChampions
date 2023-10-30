using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Vapors of Vatorr", menuName = "MarvelChampions/Card Effects/Doctor Strange/Invocations/Vapors of Valtorr")]
public class VaporsValtorr : PlayerCardEffect, IInvocation
{
    List<ICharacter> chars;

    public override bool CanActivate()
    {
        chars = new List<ICharacter>()
        {
            _owner, ScenarioManager.inst.ActiveVillain,
        };
        chars.AddRange(_owner.CardsInPlay.Allies);
        chars.AddRange(VillainTurnController.instance.MinionsInPlay);

        chars.RemoveAll(x => !x.CharStats.Afflicted());

        return chars.Count > 0;
    }

    public async Task Special()
    {
        ICharacter target = await TargetSystem.instance.SelectTarget(chars);
        List<Status> targetStatus = PopulateStatusList(target);
        Status toChange;

        if (targetStatus.Count > 1)
        {
            List<string> statuses = targetStatus.Cast<string>().ToList();
            int decision = await ChooseEffectUI.ChooseEffect(statuses);
            toChange = targetStatus[decision - 1];
        }
        else
            toChange = targetStatus[0];

        int choice;

        switch (toChange)
        {
            case Status.Stunned:
                choice = await ChooseEffectUI.ChooseEffect(new() { "Confused", "Tough" });
                target.CharStats.Attacker.Stunned = false;
                if (choice == 1)
                    target.CharStats.Confusable.Confused = true;
                else
                    target.CharStats.Health.Tough = true;
                break;
            case Status.Confused:
                choice = await ChooseEffectUI.ChooseEffect(new() { "Stunned", "Tough" });
                target.CharStats.Confusable.Confused = false;
                if (choice == 1)
                    target.CharStats.Attacker.Stunned = true;
                else
                    target.CharStats.Health.Tough = true;
                break;
            case Status.Tough:
                choice = await ChooseEffectUI.ChooseEffect(new() { "Confused", "Stunned" });
                target.CharStats.Health.Tough = false;
                if (choice == 1)
                    target.CharStats.Confusable.Confused = true;
                else
                    target.CharStats.Attacker.Stunned = true;
                break;
        }

    }

    private List<Status> PopulateStatusList(ICharacter target)
    {
        List<Status> targetStatus = new List<Status>();

        if (target.CharStats.Attacker.Stunned)
            targetStatus.Add(Status.Stunned);
        if (target.CharStats.Confusable.Confused)
            targetStatus.Add(Status.Confused);
        if (target.CharStats.Health.Tough)
            targetStatus.Add(Status.Tough);

        return targetStatus;
    }
}
