using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface ICharacter
{
    public CharacterStats CharStats { get; set; }
    public List<IAttachment> Attachments { get; set; }
}
