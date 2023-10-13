using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Guard
{
    private MinionCard _card;
    public Guard(MinionCard card)
    {
        _card = card;

        AttackSystem.Instance.Guards.Add(card);
        _card.CharStats.Health.Defeated.Add(WhenDefeated);
    }

    private Task WhenDefeated()
    {
        AttackSystem.Instance.Guards.Remove(_card);
        _card.CharStats.Health.Defeated.Remove(WhenDefeated);
        return Task.CompletedTask;
    }
}
