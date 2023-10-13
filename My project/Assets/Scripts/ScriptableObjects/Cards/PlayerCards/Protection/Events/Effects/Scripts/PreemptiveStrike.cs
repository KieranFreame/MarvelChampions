using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Preemptive Strike", menuName = "MarvelChampions/Card Effects/Protection/Events/Preemptive Strike")]
public class PreemptiveStrike : PlayerCardEffect
{
    int damage;

    public override void OnDrawn()
    {
        BoostSystem.Instance.Modifiers.Add(ReduceBoost);
    }

    public override bool CanBePlayed()
    {
        return false;
    }

    public override bool CanActivate()
    {
        if (_owner.ResourcesAvailable(Card) < Card.CardCost)
            return false;

        return true; 
    }

    public override async Task OnEnterPlay()
    {
        await _owner.CharStats.InitiateAttack(new(damage, ScenarioManager.inst.ActiveVillain, owner: _owner, card: Card));
        BoostSystem.Instance.Modifiers.Remove(ReduceBoost);
    }

    private async Task<int> ReduceBoost(int boostIcons)
    {
        if (!CanActivate()) return boostIcons;

        bool activate = await ConfirmActivateUI.MakeChoice(Card);

        if (activate)
        {
            damage = boostIcons;
            boostIcons = 0;

            await PlayCardSystem.Instance.InitiatePlayCard(new(Card));
        }

        return boostIcons;
    }

    public override void OnDiscard()
    {
        BoostSystem.Instance.Modifiers.Remove(ReduceBoost);
    }
}
