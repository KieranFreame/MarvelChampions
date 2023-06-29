using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Interrogation Room", menuName = "MarvelChampions/Card Effects/Justice/Interrogation Room")]
public class InterrogationRoom : PlayerCardEffect
{
    ICharacter _target;

    public override async Task OnEnterPlay(Player player, PlayerCard card)
    {
        _owner = player;
        Card = card;
        _owner.CharStats.AttackInitiated += AttackInitiated;

        await Task.Yield();
    }

    private void AttackInitiated()
    {
        if (ScenarioManager.sideSchemes.Count > 0 || FindObjectOfType<MainSchemeCard>().GetComponent<Threat>().CurrentThreat > 0)
            TargetSystem.TargetAcquired += CheckTarget;
    }

    private void CheckTarget(dynamic target)
    {
        TargetSystem.TargetAcquired -= CheckTarget;

        if (target is MinionCard)
        {
            _target = target;
            AttackSystem.OnAttackComplete += AttackComplete;
        }
    }

    private async void AttackComplete(Action action)
    {
        AttackSystem.OnAttackComplete -= AttackComplete;

        if (_target.CharStats.Health.CurrentHealth == 0) //defeated
        {
            bool decision = await ConfirmActivateUI.MakeChoice(Card);

            if (decision)
            {
                Card.Exhaust();
                await ThwartSystem.instance.InitiateThwart(new(1));
            }
        }
    }

    public override void OnExitPlay()
    {
        _owner.CharStats.AttackInitiated -= AttackInitiated;
    }
}
