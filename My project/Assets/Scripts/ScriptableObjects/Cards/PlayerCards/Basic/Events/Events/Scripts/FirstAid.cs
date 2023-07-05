using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "First Aid", menuName = "MarvelChampions/Card Effects/Basic/First Aid")]
public class FirstAid : PlayerCardEffect
{
    List<ICharacter> candidates = new();

    public override bool CanBePlayed()
    {
        candidates.Add(FindObjectOfType<Player>());
        candidates.AddRange((candidates[0] as Player).CardsInPlay.Allies);

        candidates.RemoveAll(x => !x.CharStats.Health.Damaged());

        return candidates.Count > 0;
    }

    public override async Task OnEnterPlay()
    {
        ICharacter target = await TargetSystem.instance.SelectTarget(candidates);
        target.CharStats.Health.RecoverHealth(2);
    }
}
