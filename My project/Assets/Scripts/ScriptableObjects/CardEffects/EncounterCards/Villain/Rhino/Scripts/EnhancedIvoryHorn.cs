using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enhanced Ivory Horn", menuName = "MarvelChampions/Card Effects/Rhino/Enhanced Ivory Horn")]
public class EnhancedIvoryHorn : EncounterCardEffect
{
    public override void OnEnterPlay(Villain owner, Card card)
    {
        _owner = owner;
        _card = card;

        _owner.CharStats.Attacker.CurrentAttack += 1;
    }

    public override void Activate()
    {
        _card.StartCoroutine(RemoveCard());
    }

    private IEnumerator RemoveCard()
    {
        yield return PayCostSystem.instance.GetResources(Resource.Physical, 3);

        if (PayCostSystem.instance.Resources.Count >= 3) //check if action was cancelled
            _owner.EncounterDeck.Discard(_card);
    }

    public override void OnExitPlay()
    {
        _owner.CharStats.Attacker.CurrentAttack -= 1;
    }
}
