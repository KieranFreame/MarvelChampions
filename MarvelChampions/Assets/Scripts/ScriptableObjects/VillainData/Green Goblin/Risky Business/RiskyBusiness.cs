using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RiskyBusiness
{
    private static RiskyBusiness instance;

    public static RiskyBusiness Instance
    {
        get
        {
            instance ??= new RiskyBusiness();
            return instance;
        }
    }

    Villain villain;
    Sprite goblinArt;
    Sprite osbornArt;

    public RiskyBusinessEnvironment environment { get; set; }

    private RiskyBusiness()
    {
        villain = ScenarioManager.inst.ActiveVillain;
        villain.gameObject.name = "Norman Osborn";
        osbornArt = Resources.Load<Sprite>("Sprites/RiskyBusiness/NormanOsborn");
        goblinArt = Resources.Load<Sprite>("Sprites/RiskyBusiness/Green_Goblin");
    }

    public void Flip()
    {
        if (villain.Name == "Norman Osborn")
        {
            villain.gameObject.name = villain.Name = "Green Goblin";
            villain.Art = goblinArt;

            //Adjust Stats
            villain.CharStats.Attacker.BaseATK = (villain.Stages.Stage == 1) ? 3 : 4;
            villain.CharStats.Attacker.CurrentAttack += villain.CharStats.Attacker.BaseATK;

            villain.CharStats.Schemer.CurrentScheme -= villain.CharStats.Schemer.BaseScheme;
            villain.CharStats.Schemer.BaseScheme = 0;
        }
        else
        {
            villain.gameObject.name = villain.Name = "Norman Osborn";
            villain.Art = osbornArt;

            //Adjust Stats
            villain.CharStats.Attacker.CurrentAttack -= villain.CharStats.Attacker.BaseATK;
            villain.CharStats.Attacker.BaseATK = 0;

            villain.CharStats.Schemer.BaseScheme = (villain.Stages.Stage == 3) ? 3 : 2;
            villain.CharStats.Schemer.CurrentScheme += villain.CharStats.Schemer.BaseScheme;
        }

        villain.GetComponent<VillainUI>().SetUI(villain);

        (villain.VillainEffect as RiskyBusinessVillain).Flip();
    }
}
