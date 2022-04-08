using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IThwarter
{
    int baseThwart { get; set; }
    int thwart { get; set; }

    void AttemptThwart();
}
