using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttachment
{
    public ICharacter Attached { get; set; }

    public void Attach() { }
    public void Detach() { }
    public void WhenRemoved() { }
}
