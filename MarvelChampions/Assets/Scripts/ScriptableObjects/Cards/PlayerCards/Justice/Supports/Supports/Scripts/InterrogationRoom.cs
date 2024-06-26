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
        GameStateManager.Instance.OnCharacterDefeated += CanRespond;
        return Task.CompletedTask;
    }

    public void CanRespond(ICharacter minion)
    {
        if (minion is MinionCard && !_card.Exhausted && ScenarioManager.inst.ThreatPresent())
        {
            EffectManager.Inst.Responding.Add(this);
        }
    }

    public override async Task Resolve()
    {
        (Card as PlayerCard).Exhaust();
        await ThwartSystem.Instance.InitiateThwart(new(1, _owner));
    }

    public override void OnExitPlay()
    {
        GameStateManager.Instance.OnCharacterDefeated -= CanRespond;
    }
}
