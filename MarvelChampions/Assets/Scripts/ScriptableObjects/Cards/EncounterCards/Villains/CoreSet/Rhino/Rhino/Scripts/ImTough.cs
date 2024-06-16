using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "I'm Tough", menuName = "MarvelChampions/Card Effects/Rhino/Im Tough")]
public class ImTough : EncounterCardEffect
{
    /// <summary>
    /// Give Rhino a tough status card. If Rhino already has a tough status card, this card gains surge.
    /// </summary>

    public override Task Resolve()
    {
        if (_owner.CharStats.Health.Tough)
        {
            ScenarioManager.inst.Surge(TurnManager.instance.CurrPlayer);
            return Task.CompletedTask;
        }

        _owner.CharStats.Health.Tough = true;
        return Task.CompletedTask;
    }
}
