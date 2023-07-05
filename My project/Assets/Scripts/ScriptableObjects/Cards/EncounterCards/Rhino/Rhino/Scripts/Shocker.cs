using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Shocker", menuName = "MarvelChampions/Card Effects/Rhino/Shocker")]
public class Shocker : EncounterCardEffect
{
    public override async Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
    {
        List<ICharacter> players = new(); 
        players.AddRange(TurnManager.Players);

        await DamageSystem.instance.ApplyDamage(new(players, 1, true));
    }
}
