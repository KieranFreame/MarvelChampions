using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Interrogation Room", menuName = "MarvelChampions/Card Effects/Justice/Interrogation Room")]
public class InterrogationRoom : PlayerCardEffect, IOptional
{
    /// <summary>
    /// After you defeat a minion, exhaust this; remove 1 threat from a scheme
    /// </summary>

    public override Task OnEnterPlay()
    {
        GameStateManager.Instance.OnCharacterDefeated += QueueEffect;
        return Task.CompletedTask;
    }

    public void QueueEffect(ICharacter minion)
    {
        if (minion.GetType() == typeof(MinionCard) && !(Card as PlayerCard).Exhausted)
        {
            EffectManager.Inst.Resolving.Push(this);
        }
    }

    public override async Task Resolve()
    {
        (Card as PlayerCard).Exhaust();
        await ThwartSystem.Instance.InitiateThwart(new(1, _owner));
    }

    public override void OnExitPlay()
    {
        GameStateManager.Instance.OnCharacterDefeated -= QueueEffect;
    }
}
