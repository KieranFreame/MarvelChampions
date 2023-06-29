using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public interface IModifyThreat
{
    Task<SchemeAction> ModifyScheme(SchemeAction action);
}
