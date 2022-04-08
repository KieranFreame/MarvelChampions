using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICombatant
{
    int baseAttack { get; set; }
    int attack { get; set; }

    void AttemptAttack();
}
