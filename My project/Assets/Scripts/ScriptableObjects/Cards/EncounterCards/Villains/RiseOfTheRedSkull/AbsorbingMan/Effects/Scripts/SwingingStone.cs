using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Swinging Stone", menuName = "MarvelChampions/Card Effects/RotRS/Absorbing Man/Swinging Stone")]
public class SwingingStone : EncounterCardEffect 
{
    public override async Task WhenRevealed(Villain owner, EncounterCard card, Player player)
    {
        _owner = owner;

        if (player.Identity.ActiveIdentity is AlterEgo) //Scheme
        {
            if (owner.VillainTraits.Contains("Stone"))
            {
                owner.CharStats.Schemer.CurrentScheme++;
                owner.CharStats.SchemeInitiated += SchemeInitiated;
            }

            await owner.CharStats.InitiateScheme();
        }
        else //Attack
        {
            if (owner.VillainTraits.Contains("Stone"))
            {
                owner.CharStats.Attacker.CurrentAttack++;
                owner.CharStats.AttackInitiated += AttackInitiated;
            }

            await owner.CharStats.InitiateAttack();
        }
    }

    private void AttackInitiated()
    {
        _owner.CharStats.Attacker.CurrentAttack--;
    }

    private void SchemeInitiated()
    {
        _owner.CharStats.Schemer.CurrentScheme--;
    }
}
