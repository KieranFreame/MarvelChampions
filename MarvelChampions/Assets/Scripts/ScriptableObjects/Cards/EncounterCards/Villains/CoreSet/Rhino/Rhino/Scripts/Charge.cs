using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Charge", menuName = "MarvelChampions/Card Effects/Rhino/Charge")]
public class Charge : EncounterCardEffect, IAttachment
{
    public ICharacter Attached { get; set; }

    public override async Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
    {
        Attached = _owner = owner;
        Card = card;

        Attach();

        AttackSystem.Instance.OnAttackCompleted.Add(IsTriggerMet);

        await Task.Yield();
    }

    public void Attach()
    {
        Attached.CharStats.Attacker.CurrentAttack += 3;
        Attached.CharStats.Attacker.Keywords.Add("Overkill");
    }

    private void IsTriggerMet(AttackAction action)
    {
        if (action.Owner == _owner as ICharacter)
        {
            Detach();

            AttackSystem.Instance.OnAttackCompleted.Remove(IsTriggerMet);
            ScenarioManager.inst.EncounterDeck.Discard(Card);
        }
    }

    public void Detach()
    {
        Attached.CharStats.Attacker.CurrentAttack -= 3;
        Attached.CharStats.Attacker.Keywords.Remove("Overkill");
    }
}
