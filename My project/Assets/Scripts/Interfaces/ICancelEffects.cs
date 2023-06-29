using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public interface ICancelEffect
{
    public Task<bool> CancelEffect(ICard cardToCancel);
}
