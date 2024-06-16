using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Hard To Keep Down", menuName = "MarvelChampions/Card Effects/Rhino/Hard To Keep Down")]
public class HardToKeepDown : EncounterCardEffect
{
    /// <summary>
    /// Rhino heals 4 damage. If no damage was healed this way, this card gains Surge.
    /// </summary>
    
    public override Task Resolve()
    {
        var health = _owner.CharStats.Health;
        var player = TurnManager.instance.CurrPlayer;

        if (!health.Damaged())
        {
            ScenarioManager.inst.Surge(player);
        }
        else
        {
            health.CurrentHealth += 4;
        }

       
        return Task.CompletedTask;
    }
}
