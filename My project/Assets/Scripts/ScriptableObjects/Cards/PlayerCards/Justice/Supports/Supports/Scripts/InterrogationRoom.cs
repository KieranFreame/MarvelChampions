using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Interrogation Room", menuName = "MarvelChampions/Card Effects/Justice/Interrogation Room")]
public class InterrogationRoom : PlayerCardEffect
{
    MinionCard _target;

    public override Task OnEnterPlay()
    {
        _owner.CharStats.AttackInitiated += AttackInitiated;
        return Task.CompletedTask;
    }

    private void AttackInitiated()
    {
        if (_target != null)
            _target.CharStats.Health.Defeated.Remove(Defeated);

        if (ScenarioManager.sideSchemes.Count > 0 || ScenarioManager.inst.MainScheme.Threat.CurrentThreat > 0)
            AttackSystem.TargetAcquired += CheckTarget;
    }

    private void CheckTarget(ICharacter target)
    {
        AttackSystem.TargetAcquired -= CheckTarget;

        if (target is MinionCard)
        {
            _target = target as MinionCard;
            _target.CharStats.Health.Defeated.Add(Defeated);
        }
    }

    private async Task Defeated()
    {
        _target.CharStats.Health.Defeated.Remove(Defeated);

        bool decision = await ConfirmActivateUI.MakeChoice(Card);

        if (decision)
        {
            Card.Exhaust();
            await ThwartSystem.Instance.InitiateThwart(new(1));
        }
    }

    public override void OnExitPlay()
    {
        _owner.CharStats.AttackInitiated -= AttackInitiated;
    }
}
