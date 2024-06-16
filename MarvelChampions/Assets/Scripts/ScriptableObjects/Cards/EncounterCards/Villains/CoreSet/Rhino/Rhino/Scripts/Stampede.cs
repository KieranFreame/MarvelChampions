using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[CreateAssetMenu(fileName = "Stampede", menuName = "MarvelChampions/Card Effects/Rhino/Stampede")]
public class Stampede : EncounterCardEffect
{
    /// <summary>
    /// When Revealed (Alter-Ego): This card gains surge.
    /// When Revealed(Hero): Rhino attacks you.If a character is damaged by this attack, that character is stunned.
    /// </summary>

    public override async Task Resolve()
    {
        var player = TurnManager.instance.CurrPlayer;
        var identity = player.Identity.ActiveIdentity;

        if (identity is AlterEgo)
        {
            ScenarioManager.inst.Surge(player);
        }
        else
        {
            GameStateManager.Instance.OnActivationCompleted += AttackComplete;
            await _owner.CharStats.InitiateAttack();
        }
    }

    private void AttackComplete(Action action)
    {
        GameStateManager.Instance.OnActivationCompleted -= AttackComplete;

        if (((AttackAction)action).Target.CharStats.Health.CurrentHealth > 0)
        {
            ((AttackAction)action).Target.CharStats.Attacker.Stunned = true;
        }
    }
}
