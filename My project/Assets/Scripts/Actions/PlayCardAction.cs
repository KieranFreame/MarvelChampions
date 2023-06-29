using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayCardAction : Action
{
    public List<PlayerCard> Candidates { get; private set; } = new List<PlayerCard>();
    public PlayerCard CardToPlay { get; private set; }

    public PlayCardAction(ICharacter owner, List<PlayerCard> candidates, PlayerCard cardToPlay) : base ()
    {
        Owner = owner;
        Candidates = candidates;
        CardToPlay = cardToPlay;
    }
}
