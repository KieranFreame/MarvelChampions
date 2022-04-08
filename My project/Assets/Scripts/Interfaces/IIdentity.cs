using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IIdentity
{
    string idName { get; set; }
    int baseHandSize { get; set; }
    int handSize { get; set; }
    void SwitchIdentity();
}
