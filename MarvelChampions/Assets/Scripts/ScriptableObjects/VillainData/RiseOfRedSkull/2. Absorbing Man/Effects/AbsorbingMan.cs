using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using System.Threading.Tasks;

[CreateAssetMenu(fileName = "Absorbing Man", menuName = "MarvelChampions/Villain Effects/RotRS/Absorbing Man")]
public class AbsorbingMan : VillainEffect
{
    public override void LoadEffect(Villain owner)
    {
        base.LoadEffect(owner);

        NoneShallPass.EnvironmentChanged += ChangeTraits;
    }

    private void ChangeTraits(EncounterCard newEnvironment)
    {
        _owner.VillainTraits.RemoveItem(NoneShallPass.CurrentEnvironment.CardTraits.Collection[0]);
        _owner.VillainTraits.AddItem(newEnvironment.CardTraits.Collection[0]);
    }

    #region Stage Two
    public override async Task StageTwoEffect()
    {
        EncounterCardData superAbsorbingPower = ScenarioManager.inst.EncounterDeck.Search("Super Absorbing Power", true) as EncounterCardData;

        if (superAbsorbingPower == null)
            return;

        var card = CreateCardFactory.Instance.CreateCard(superAbsorbingPower, GameObject.Find("SideSchemeTransform").transform) as SchemeCard;

        ScenarioManager.sideSchemes.Add(card);
        await card.OnRevealCard();
    }
    #endregion

    #region Stage Three
    public override Task StageThreeEffect()
    {
        VillainTurnController.instance.OnActivationComplete.Add(ApplyTraitEffect);
        return Task.CompletedTask;
    }

    async Task ApplyTraitEffect()
    {
        if (_owner.VillainTraits.Contains("Ice") || _owner.VillainTraits.Contains("Stone"))
        {
            ScenarioManager.inst.MainScheme.Threat.GainThreat(1);
        }

        if(_owner.VillainTraits.Contains("Metal") || _owner.VillainTraits.Contains("Wood"))
        {
            List<ICharacter> targets = new() { TurnManager.instance.CurrPlayer };
            targets.AddRange((targets[0] as Player).CardsInPlay.Allies);

            await IndirectDamageHandler.inst.HandleIndirectDamage(targets, 1);
        }
    }
    #endregion
}
