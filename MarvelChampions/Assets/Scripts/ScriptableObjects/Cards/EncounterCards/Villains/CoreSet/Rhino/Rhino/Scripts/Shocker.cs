using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Shocker", menuName = "MarvelChampions/Card Effects/Rhino/Shocker")]
public class Shocker : EncounterCardEffect
{
    public override async Task Resolve()
    {
        List<ICharacter> players = new(); 
        players.AddRange(TurnManager.Players);

        await DamageSystem.Instance.ApplyDamage(new(players, 1, targetAll:true, isAttack:false, owner:(MinionCard)_card));
    }
}
