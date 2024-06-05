using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Backflip", menuName = "MarvelChampions/Card Effects/Spider-Man (Peter Parker)/Backflip")]
public class Backflip : PlayerCardEffect
{
    public override void OnDrawn()
    {
        _owner.CharStats.Health.Modifiers.Add(OnTakeDamage);
    }

    public override async Task OnEnterPlay()
    {
        _owner.CharStats.Health.Modifiers.Remove(OnTakeDamage);
        await Task.Yield();
    }

    public async Task<DamageAction> OnTakeDamage(DamageAction action)
    {
        bool decision = await ConfirmActivateUI.MakeChoice(Card);

        if (decision)
        {
            await PlayCardSystem.Instance.InitiatePlayCard(new(_card));
            action.Value = 0;
        }

        return action;
    }

    public override void OnDiscard()
    {
        _owner.CharStats.Health.Modifiers.Remove(OnTakeDamage);
    }
}
