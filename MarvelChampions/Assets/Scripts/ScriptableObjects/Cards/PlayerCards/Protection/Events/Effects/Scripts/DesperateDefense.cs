using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Desperate Defense", menuName = "MarvelChampions/Card Effects/Protection/Events/Desperate Defense")]
public class DesperateDefense : PlayerCardEffect
{
    public override void OnDrawn()
    {
        _owner.CharStats.Defender.modifiers.Add(ModifyDefence);
    }

    private async Task<int> ModifyDefence(int defence)
    {
        if (!CanBePlayed()) return defence;
        if (_owner.ResourcesAvailable(_card) < _card.CardCost) return defence;

        if (await ConfirmActivateUI.MakeChoice(_card))
        {
            await PlayCardSystem.Instance.InitiatePlayCard(new(_card));
            defence += 2;
            _owner.CharStats.Health.OnTakeDamage += OnTakeDamage;
        }

        return defence;
    }

    private void OnTakeDamage(DamageAction arg0)
    {
        _owner.CharStats.Health.OnTakeDamage -= OnTakeDamage;

        if (arg0.Value == 0)
        {
            _owner.Ready();
        }
    }

    public override void OnDiscard()
    {
        _owner.CharStats.Defender.modifiers.Remove(ModifyDefence);
    }
}
