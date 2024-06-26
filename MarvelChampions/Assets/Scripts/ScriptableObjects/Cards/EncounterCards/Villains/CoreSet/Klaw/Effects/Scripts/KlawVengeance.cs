using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Klaw's Vengeance", menuName = "MarvelChampions/Card Effects/Klaw/Klaw's Vengeance")]
public class KlawVengeance : EncounterCardEffect
{
    int charHP;
    ICharacter _target;
    public override async Task Resolve()
    {
        var player = TurnManager.instance.CurrPlayer;

        if (player.Identity.ActiveIdentity is AlterEgo)
        {
            Debug.Log("Discarding 1 Card from your hand");
            var pCard = player.Hand.cards[Random.Range(0, player.Hand.cards.Count)];

            Debug.Log("Discarded " + pCard.CardName);

            player.Hand.Discard(pCard);
        }
        else //hero
        {
            _owner.CharStats.AttackInitiated += AttackInitiated;
            await _owner.CharStats.InitiateAttack();
        }
    }

    private void AttackInitiated() => DefendSystem.Instance.OnTargetSelected += SetDefender;

    private void SetDefender(ICharacter target)
    {
        DefendSystem.Instance.OnTargetSelected -= SetDefender;
        _target = target;
        _target.CharStats.Health.OnTakeDamage += OnTakeDamage;
    }

    private void OnTakeDamage(DamageAction action)
    {
        if (action.Value > 0)
        {
            ScenarioManager.inst.MainScheme.Threat.GainThreat(1);
        }

        _target.CharStats.Health.OnTakeDamage -= OnTakeDamage;
    }
}
