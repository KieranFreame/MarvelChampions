using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayCardAction : Action
{
    public PlayerCard CardToPlay { get; private set; }

    public PlayCardAction(PlayerCard cardToPlay) : base ()
    {
        Owner = cardToPlay.Owner;
        CardToPlay = cardToPlay;
    }
}
