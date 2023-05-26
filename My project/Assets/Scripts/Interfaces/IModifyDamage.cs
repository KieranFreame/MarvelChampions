using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IModifyDamage
{
    IEnumerator OnTakeDamage(DamageAction action, System.Action<DamageAction> callback);
}
