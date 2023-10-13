using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Ultron", menuName = "MarvelChampions/Villain Effects/Ultron")]
public class Ultron : VillainEffect
{
    public UltronDrones ultronDrone;

    private void AttackInitiated()
    {
        if (_owner.Stages.Stage == 2)
        {
            Player p = TurnManager.instance.CurrPlayer;

            ultronDrone.SpawnDrone(p);

            _owner.CharStats.Attacker.CurrentAttack += 1 * VillainTurnController.instance.MinionsInPlay.Where(x => x.CardTraits.Contains("Drone")).Count();
        }

        AttackSystem.Instance.OnAttackCompleted.Add(OnAttackComplete);
    }
    private async Task OnAttackComplete(Action action)
    {
        AttackSystem.Instance.OnAttackCompleted.Remove(OnAttackComplete);

        if (_owner.Stages.Stage == 1)
        {
            var attack = action as AttackAction;

            int decision = await ChooseEffectUI.ChooseEffect(new List<string>() { "Place 1 threat on the main scheme", "The top card of your deck becomes a Drone" });

            if (decision == 1)
            {
                ScenarioManager.inst.MainScheme.Threat.GainThreat(1);
            }
            else
            {
                Player p = (attack.Target is not Player) ? (attack.Target as AllyCard).Owner : attack.Target as Player;
                ultronDrone.SpawnDrone(p);
            }
        }
        else if (_owner.Stages.Stage == 2)
        {
            _owner.CharStats.Attacker.CurrentAttack -= 1 * VillainTurnController.instance.MinionsInPlay.Where(x => x.CardTraits.Contains("Drone")).Count();
        }
    }
    public override Task StageOneEffect()
    {
        _owner.CharStats.AttackInitiated += AttackInitiated;
        return Task.CompletedTask;
    }

    #region Stage Three
    public override async Task StageThreeEffect()
    {
        _owner.CharStats.AttackInitiated -= AttackInitiated;

        EncounterCardData data = ScenarioManager.inst.EncounterDeck.Search("Ultron's Imperative", true) as EncounterCardData;

        if (data != null)
        {
            SchemeCard imperative = CreateCardFactory.Instance.CreateCard(data, GameObject.Find("SideSchemeTransform").transform) as SchemeCard;
            ScenarioManager.sideSchemes.Add(imperative);
            await imperative.OnRevealCard();
        }

        foreach (var drone in VillainTurnController.instance.MinionsInPlay.Where(x => x.CardTraits.Contains("Drone")))
        {
            drone.CharStats.Attacker.CurrentAttack++;
            drone.CharStats.Health.IncreaseMaxHealth(1);
        }

        _owner.CharStats.Health.Modifiers.Add(OnTakeDamage);
        VillainTurnController.instance.MinionsInPlay.CollectionChanged += MinionAdded;
    }

    private void MinionAdded(object sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action is NotifyCollectionChangedAction.Add)
        {
            foreach (var item in e.NewItems)
            {
                if ((item as MinionCard).CardTraits.Contains("Drone"))
                {
                    (item as MinionCard).CharStats.Attacker.CurrentAttack++;
                    (item as MinionCard).CharStats.Health.IncreaseMaxHealth(1);
                }
            }
        }
    }

    private Task<DamageAction> OnTakeDamage(DamageAction action)
    {
        if (VillainTurnController.instance.MinionsInPlay.Any(x => x.CardTraits.Contains("Drone")))
            action.Value = 0;

        return Task.FromResult(action);
    }
    #endregion
}
