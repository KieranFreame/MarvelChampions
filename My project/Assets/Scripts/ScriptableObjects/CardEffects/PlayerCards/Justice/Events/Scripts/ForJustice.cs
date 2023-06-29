using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "For Justice", menuName = "MarvelChampions/Card Effects/Justice/For Justice")]
public class ForJustice : PlayerCardEffect
{
    public override async Task OnEnterPlay(Player player, PlayerCard card)
    {
        _owner = player;
        Card = card;

        if (PayCostSystem.instance.Resources.Contains(Resource.Scientific))
        {
            await _owner.CharStats.InitiateThwart(new(4));
        }
        else
        {
            await _owner.CharStats.InitiateThwart(new(3));
        }
    }
}
