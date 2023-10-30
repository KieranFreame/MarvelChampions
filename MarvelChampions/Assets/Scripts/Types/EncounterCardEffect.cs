using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class EncounterCardEffect : ScriptableObject
{
    protected Villain _owner;
    public string EffectID { get; set; }
    public EncounterCard Card { get; set; }

    public virtual bool CanActivate(Player player) { return false; }
    public virtual async Task OnEnterPlay(Villain owner, EncounterCard card, Player player) { await Task.Yield(); }
    public virtual async Task OnExitPlay() { await Task.Yield(); }
    public virtual async Task WhenRevealed(Villain owner, EncounterCard card, Player player) { await OnEnterPlay(owner, card, player); }
    public virtual async Task Activate(Player player) { await Task.Yield(); }
    public virtual async Task Boost(Action action) { await Task.Yield(); }
    public virtual async Task WhenDefeated() { await Task.Yield(); }
    public virtual Task WhenCompleted() { return Task.CompletedTask; }
}
