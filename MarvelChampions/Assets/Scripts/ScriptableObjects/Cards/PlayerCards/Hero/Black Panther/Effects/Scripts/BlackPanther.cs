using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BlackPanther", menuName = "MarvelChampions/Identity Effects/Black Panther/Hero")]
public class BlackPanther : IdentityEffect
{
    Retaliate _retaliate;

    public override void LoadEffect(Player _owner)
    {
        owner = _owner;
        _retaliate = new(owner, 1);

        _retaliate.OnFlip(false);
    }

    public override void OnFlipUp()
    {
        _retaliate.OnFlip(true);
    }

    public override void OnFlipDown()
    {
        _retaliate.OnFlip(false);
    }
}
