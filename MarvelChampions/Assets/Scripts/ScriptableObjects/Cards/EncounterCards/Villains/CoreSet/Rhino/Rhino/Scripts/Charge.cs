using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Charge", menuName = "MarvelChampions/Card Effects/Rhino/Charge")]
public class Charge : EncounterCardEffect, IAttachment
{
    /// <summary>
    /// Rhino's next attack gains Overkill
    /// </summary>

    public ICharacter Attached { get; set; }

    public override Task Resolve()
    {
        Attached = _owner;

        Attach();

        GameStateManager.Instance.OnActivationCompleted += IsTriggerMet;

        return Task.CompletedTask;
    }

    public void Attach()
    {
        Attached.CharStats.Attacker.CurrentAttack += 3;
        Attached.CharStats.Attacker.Keywords.Add(Keywords.Overkill);
    }

    private void IsTriggerMet(Action action)
    {
        if (action is AttackAction && action.Owner.Name == "Rhino")
        {
            Detach();

            GameStateManager.Instance.OnActivationCompleted -= IsTriggerMet;
            ScenarioManager.inst.EncounterDeck.Discard(Card);
        }
    }

    public void Detach()
    {
        Attached.CharStats.Attacker.CurrentAttack -= 3;
        Attached.CharStats.Attacker.Keywords.Remove(Keywords.Overkill);
    }
}
