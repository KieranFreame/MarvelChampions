using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDestructable
{
    Health hitpoints { get; set; }

    void WhenDefeated();
}
