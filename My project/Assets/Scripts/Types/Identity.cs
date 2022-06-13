using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Identity
{
    public HeroData hero;
    public AlterEgoData alterEgo;
    public HeroSetData heroSet;

    public IdentityType activeIdentity = IdentityType.AlterEgo;

    public bool hasFlipped { get; set; } = false;
    public bool exhausted { get; set; } = false;

    public void Flip()
    {
        if (activeIdentity == IdentityType.Hero)
            activeIdentity = IdentityType.AlterEgo;
        else
            activeIdentity = IdentityType.Hero;

        hasFlipped = true;
    }

    
}
