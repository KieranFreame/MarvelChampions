using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public interface IModifyDamage
{
    Task<DamageAction> OnTakeDamage(DamageAction action, ICharacter target);
}
