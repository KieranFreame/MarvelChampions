using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ResourceCardEffect : PlayerCardEffect
{
    public virtual List<Resource> GetResources() { return Card.Resources; }
    public virtual int ResourceCount(PlayerCard card) { return Card.Resources.Count; }
    public virtual async Task WhenSpent() { await Task.Yield(); }
}
