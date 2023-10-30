using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public abstract class VillainEffect : ScriptableObject
{
    protected Villain _owner;
    public virtual void LoadEffect(Villain owner) { _owner = owner; _owner.gameObject.name = _owner.VillainName; }
    public virtual Task StageOneEffect() { return Task.CompletedTask; }
    public virtual Task StageTwoEffect() { return Task.CompletedTask; }
    public virtual Task StageThreeEffect() { return Task.CompletedTask; }
    public virtual Task FlipUp() { return Task.CompletedTask; }
    public virtual Task FlipDown() { return Task.CompletedTask; }
}
