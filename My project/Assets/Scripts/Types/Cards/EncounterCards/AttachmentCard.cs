using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachmentCard : EncounterCard, IAttachment
{
    private ICharacter _attached;
    public ICharacter Attached { get => _attached; set => _attached = value; }

    private int _atkModifier = 0;
    private int _thwSchModifier = 0;

    public override void LoadCardData(EncounterCardData data, Villain owner)
    {
        _atkModifier = (data as AttachmentCardData).ATKModifier;
        _thwSchModifier = (data as AttachmentCardData).THWSCHModifier;

        base.LoadCardData(data, owner);
    }

    public int ATKModifier
    {
        get => _atkModifier;
    }

    public int THWSCHModifier
    {
        get => _thwSchModifier;
    }
}
