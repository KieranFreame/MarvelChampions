using System.Threading.Tasks;
using UnityEngine;

public class PlayerCardEffect : ScriptableObject, IEffect
{
    protected Player _owner;
    public ICharacter Owner { get => _owner; set => _owner = (Player)value; }

    protected PlayerCard _card;
    public ICard Card { get => _card; set => _card = (PlayerCard)value; }
    protected bool HasActivated { get; set; }

    public virtual void LoadEffect(Player player, PlayerCard card) { _owner = player; _card = card; }

    public virtual void OnDrawn() { }
    public virtual bool CanActivate() { return false; }
    public virtual bool CanBePlayed() { 
        if (Card.CurrZone != Zone.Hand) return false; 
        return true; 
    }
    public virtual bool CanResolve() {  return true; }

    public virtual Task OnEnterPlay() { return Task.CompletedTask; }
    public virtual Task Activate() { return Task.CompletedTask; }
    public virtual Task Resolve() { return Task.CompletedTask; }
    public virtual Task WhenDefeated() { return Task.CompletedTask; }
    public virtual void OnExitPlay() { }
    public virtual void OnDiscard() { }
}
