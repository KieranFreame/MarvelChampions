using System.Threading.Tasks;
using UnityEngine;

public class PlayerCardEffect : ScriptableObject
{
    protected Player _owner;
    public string EffectID { get; set; }
    public PlayerCard Card { get; set; }
    protected bool HasActivated { get; set; }

    public virtual bool CanActivate() { return false; }
    public virtual bool CanAttack() { return !Card.Exhausted; }
    public virtual bool CanThwart() { return !Card.Exhausted; }
    public virtual bool CanDefend() { return !Card.Exhausted; }
    public virtual void LoadEffect(Player player, PlayerCard card) { _owner = player; Card = card; }
    public virtual void OnDrawn() { }
    public virtual bool CanBePlayed() { 
        if (Card.CurrZone != Zone.Hand) return false; 
        return true; 
    }
    public virtual Task OnEnterPlay() { return Task.CompletedTask; }
    public virtual Task Activate() { return Task.CompletedTask; }
    public virtual Task WhenDefeated() { return Task.CompletedTask; }
    public virtual void OnExitPlay() { }
    public virtual void OnDiscard() { }
}
