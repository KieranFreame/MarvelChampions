using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard
{
    private dynamic _owner;
    private MinionCard _card;
    public Guard(dynamic owner, MinionCard card)
    {
        _owner = owner;
        _card = card;

        TargetSystem.CheckGuard += Effect;
        _card.CharStats.Health.Defeated += WhenDefeated;
    }

    private void Effect(List<ICharacter> candidates)
    {
        candidates.RemoveAll(x => x == _owner);
    }

    private void WhenDefeated()
    {
        TargetSystem.CheckGuard -= Effect;
        _card.CharStats.Health.Defeated -= WhenDefeated;
    }
}
