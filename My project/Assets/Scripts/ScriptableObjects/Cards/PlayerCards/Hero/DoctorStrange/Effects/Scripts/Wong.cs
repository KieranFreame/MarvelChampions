using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Wong", menuName = "MarvelChampions/Card Effects/Doctor Strange/Wong")]
public class Wong : PlayerCardEffect
{
    public override bool CanActivate()
    {
        if (Card.Exhausted) return false;

        return true;
    }

    public override async Task Activate()
    {
        Card.Exhaust();

        if (_owner.CharStats.Health.Damaged())
        {
            int decision = await ChooseEffectUI.ChooseEffect(new() { "Heal 1 damage from your identity", "Discard the top card of the Invocation deck" });

            if (decision == 1)
            {
                _owner.CharStats.Health.RecoverHealth(1);
                return;
            }
        }

        InvocationDeck.Instance.Discard();
    }
}
