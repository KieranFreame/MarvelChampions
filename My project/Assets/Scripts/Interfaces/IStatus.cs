using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStatus
{
    bool stunned { get; set; }
    bool confused { get; set; }
    bool tough { get; set; }
}
