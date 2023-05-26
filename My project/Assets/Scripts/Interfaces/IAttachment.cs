using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttachment
{
    IAttached Attached { get; set; }
}
