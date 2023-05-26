using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttached
{
    List<IAttachment> Attachments { get; }
}
