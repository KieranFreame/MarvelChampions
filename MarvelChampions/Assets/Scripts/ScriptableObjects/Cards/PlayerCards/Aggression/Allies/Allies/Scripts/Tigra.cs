using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Tigra", menuName = "MarvelChampions/Card Effects/Aggression/Tigra")]
public class Tigra : PlayerCardEffect
{
    public override Task OnEnterPlay()
    {
        GameStateManager.Instance.OnActivationCompleted += IsTriggerMet;
        return Task.CompletedTask;
    }

    private void IsTriggerMet(Action action)
    {
        if (action is not AttackAction || action.Owner.Name != "Tigra") return;

        var attack = (AttackAction)action;

        if (attack.Target is MinionCard && attack.Target.CharStats.Health.CurrentHealth <= 0)
        {
            (Card as AllyCard).CharStats.Health.CurrentHealth++;
        }
    }

    public override void OnExitPlay()
    {
        GameStateManager.Instance.OnActivationCompleted -= IsTriggerMet;
    }
}
