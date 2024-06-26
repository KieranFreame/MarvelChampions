using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ResourceCardEffect : PlayerCardEffect
{
    public virtual void GetResources() { }
    public virtual int ResourceCount(PlayerCard card) { return _card.Resources.Count; }
    public virtual async Task WhenSpent() { await Task.Yield(); }
}
