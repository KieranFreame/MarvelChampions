using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Chase Them Down", menuName = "MarvelChampions/Card Effects/Aggression/Chase Them Down")]
public class ChaseThemDown : PlayerCardEffect
{
    ICharacter _target;

    public override void OnDrawn()
    {
        _owner.CharStats.AttackInitiated += AttackInitiated;
    }

    public override async Task OnEnterPlay()
    {
        _owner.CharStats.AttackInitiated += AttackInitiated;

        await _owner.CharStats.InitiateThwart(new(2));
    }

    private void AttackInitiated()
    {
        if (_target != null)
            _target.CharStats.Health.Defeated.Remove(Defeated);

        //Don't continue if there is no threat to remove
        if (ScenarioManager.sideSchemes.Count > 0 || ScenarioManager.inst.MainScheme.Threat.CurrentThreat > 0)
            AttackSystem.TargetAcquired += CheckTarget;
    }

    private void CheckTarget(ICharacter target)
    {
        AttackSystem.TargetAcquired -= CheckTarget;
        _target = target;
        _target.CharStats.Health.Defeated.Add(Defeated);
    }

    private async Task Defeated()
    {
        _target.CharStats.Health.Defeated.Remove(Defeated);

        bool decision = await ConfirmActivateUI.MakeChoice(Card);

        if (decision)
        {
            await PlayCardSystem.Instance.InitiatePlayCard(new(Card));
        }
        
    }

    public override void OnDiscard()
    {
        _owner.CharStats.AttackInitiated += AttackInitiated;
    }
}
