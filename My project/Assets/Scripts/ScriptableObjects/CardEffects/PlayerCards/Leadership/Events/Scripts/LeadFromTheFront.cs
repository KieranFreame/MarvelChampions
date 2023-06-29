using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Lead From The Front", menuName = "MarvelChampions/Card Effects/Leadership/Lead From The Front")]
public class LeadFromTheFront : PlayerCardEffect
{
    public override async Task OnEnterPlay(Player player, PlayerCard card)
    {
        _owner = player;
        Card = card;

        _owner.CharStats.Attacker.CurrentAttack++;
        _owner.CharStats.Thwarter.CurrentThwart++;

        foreach (AllyCard a in _owner.CardsInPlay.Allies)
        {
            a.CharStats.Attacker.CurrentAttack++;
            a.CharStats.Thwarter.CurrentThwart++;
        }

        TurnManager.OnEndPlayerPhase += OnEndPhase;

        await Task.Yield();
    }

    private void OnEndPhase()
    {
        TurnManager.OnEndPlayerPhase -= OnEndPhase;

        _owner.CharStats.Attacker.CurrentAttack--;
        _owner.CharStats.Thwarter.CurrentThwart--;

        foreach (AllyCard a in _owner.CardsInPlay.Allies)
        {
            a.CharStats.Attacker.CurrentAttack--;
            a.CharStats.Thwarter.CurrentThwart--;
        }
    }
}
