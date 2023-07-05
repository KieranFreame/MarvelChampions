using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public interface IModifyDamage
{
    Task<int> OnTakeDamage(int damage);
}
