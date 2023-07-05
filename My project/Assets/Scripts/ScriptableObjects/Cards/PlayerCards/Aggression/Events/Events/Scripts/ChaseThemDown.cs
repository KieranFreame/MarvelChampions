using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Chase Them Down", menuName = "MarvelChampions/Card Effects/Aggression/Chase Them Down")]
public class ChaseThemDown : PlayerCardEffect
{
    ICharacter _target;

    public override void OnDrawn(Player player, PlayerCard card)
    {
        _owner = player;
        Card = card;
        _owner.CharStats.AttackInitiated += AttackInitiated;
    }

    public override async Task OnEnterPlay()
    {
        _owner.CharStats.AttackInitiated -= AttackInitiated;

        await _owner.CharStats.InitiateThwart(new(2));
    }

    private void AttackInitiated()
    {
        //Don't continue if there is no threat to remove
        if (ScenarioManager.sideSchemes.Count > 0 || FindObjectOfType<MainSchemeCard>().GetComponent<Threat>().CurrentThreat > 0)
            TargetSystem.TargetAcquired += CheckTarget;
    }

    private void CheckTarget(dynamic target)
    {
        TargetSystem.TargetAcquired -= CheckTarget;
        _target = target;
        AttackSystem.OnAttackComplete += AttackComplete;
    }

    private async void AttackComplete(Action action)
    {
        AttackSystem.OnAttackComplete -= AttackComplete;

        if (_target.CharStats.Health.CurrentHealth == 0) //defeated
        {
            bool decision = await ConfirmActivateUI.MakeChoice(Card);

            if (decision)
            {
                await PlayCardSystem.instance.InitiatePlayCard(new(_owner, _owner.Hand.cards, Card));
            }
        }
    }

    public override void OnDiscard()
    {
        _owner.CharStats.AttackInitiated -= AttackInitiated;
    }
}
