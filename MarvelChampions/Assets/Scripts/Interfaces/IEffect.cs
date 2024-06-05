using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public interface IEffect
{
    public ICharacter Owner { get; set; }
    public ICard Card { get; set; }

    public virtual bool CanActivate() { return false; }
    public virtual bool CanResolve() { return false; }
    public virtual void LoadEffect(ICharacter owner, ICard card) { Owner = owner; Card = card; }

    public virtual void OnEnterPlay(Player player = null) { }

    public virtual Task Activate() { return Task.CompletedTask; }
    public virtual Task Resolve() { return Task.CompletedTask; }

    public virtual void OnExitPlay() { }
    
}
