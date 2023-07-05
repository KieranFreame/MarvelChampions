using System.Threading.Tasks;
using UnityEngine;

public class PlayerCardEffect : ScriptableObject
{
    protected Player _owner;
    public string EffectID { get; set; }
    public PlayerCard Card { get; set; }
    protected bool HasActivated { get; set; }

    public virtual bool CanActivate() { return false; }
    public virtual void OnDrawn(Player player, PlayerCard card) { _owner = player; Card = card; }
    public virtual bool CanBePlayed() { return true; }
    public virtual async Task OnEnterPlay() { await Task.Yield(); }
    public virtual async Task Activate() { await Task.Yield(); }
    public virtual void WhenDefeated() { }
    public virtual void OnExitPlay() { }
    public virtual void OnDiscard() { }
}
