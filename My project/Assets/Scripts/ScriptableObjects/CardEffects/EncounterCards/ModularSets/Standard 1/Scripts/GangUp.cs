using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using UnityEngine;

[CreateAssetMenu(fileName = "Gang Up", menuName = "MarvelChampions/Card Effects/Standard I/Gang Up")]
public class GangUp : EncounterCardEffect
{
    public override void OnEnterPlay(Villain owner, Card card)
    {
        _card = card;
        _owner = owner;
        Player player = FindObjectOfType<Player>();

        if (player.Identity.ActiveIdentity is Hero)
        {
            _card.StartCoroutine(HandleAttacks());
        }
        else
        {
            owner.Surge(player);
        } 
    }

    private IEnumerator HandleAttacks()
    {
        yield return _card.StartCoroutine(_owner.CharStats.InitiateAttack());

        List<MinionCard> minions = new();
        minions.AddRange(VillainTurnController.instance.MinionTransform.GetComponentsInChildren<MinionCard>());

        foreach (MinionCard m in minions)
        {
            yield return _card.StartCoroutine(m.CharStats.InitiateAttack());
        }
    }
}
