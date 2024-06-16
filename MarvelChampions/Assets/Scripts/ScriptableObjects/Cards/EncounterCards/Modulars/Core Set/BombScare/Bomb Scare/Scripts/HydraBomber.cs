using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Hydra Bomber", menuName = "MarvelChampions/Card Effects/Bomb Scare/Hydra Bomber")]
public class HydraBomber : EncounterCardEffect
{
    Player target;
    [SerializeField] private string takeDamage;
    [SerializeField] private string placeThreat;
    
    /// <summary>
    /// When Revealed: Choose to either take 2 damage, or place 1 threat on the main scheme
    /// </summary>

    public override async Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
    {
       await EffectManager.Inst.AddEffect(_card, this);
    }

    public override async Task Resolve()
    {
        target = TurnManager.instance.CurrPlayer;
        int effectIndex = await ChooseEffectUI.ChooseEffect(new List<string> { takeDamage, placeThreat });

        switch (effectIndex)
        {
            case 1:
                await DamageSystem.Instance.ApplyDamage(new DamageAction(target, 2, card: Card));
                break;
            case 2:
                ScenarioManager.inst.MainScheme.Threat.GainThreat(1);
                break;
        }
    }
}
