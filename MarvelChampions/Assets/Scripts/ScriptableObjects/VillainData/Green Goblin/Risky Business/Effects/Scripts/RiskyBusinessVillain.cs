using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Risky Business", menuName = "MarvelChampions/Villain Effects/Risky Business")]
public class RiskyBusinessVillain : VillainEffect
{
    public override void LoadEffect(Villain owner)
    {
        _owner = owner;

        _owner.CharStats.Attacker.AttackCancel.Add(CancelAttack);
        _owner.CharStats.Health.Modifiers.Add(DamageCancel);
    }

    public async void Flip()
    {
        if (_owner.Name == "Norman Osborn")
        {
            _owner.CharStats.Attacker.AttackCancel.Add(CancelAttack);
            _owner.CharStats.Health.Modifiers.Add(DamageCancel);

            _owner.CharStats.Schemer.SchemeCancel.Remove(CancelScheme);
        }
        else
        {
            _owner.CharStats.Schemer.SchemeCancel.Add(CancelScheme);
            await WhenRevealed();

            _owner.CharStats.Attacker.AttackCancel.Remove(CancelAttack);
            _owner.CharStats.Health.Modifiers.Remove(DamageCancel);
        }
    }

    #region Norman Osborn
    private Task<DamageAction> DamageCancel(DamageAction action)
    {
        int value = action.Value;
        RiskyBusiness.Instance.environment.RemoveCounters(value);
        action.Value = 0;
        return Task.FromResult(action);
    }

    private Task<AttackAction> CancelAttack(AttackAction action)
    {
        RiskyBusiness.Instance.environment.AddCounters(_owner.Stages.Stage);
        action = null;
        return Task.FromResult(action);
    }
    #endregion

    #region Green Goblin
    private async Task WhenRevealed()
    {
        switch (_owner.Stages.Stage)
        {
            case 1:
                foreach (var p in TurnManager.Players.Where(x => x.Identity.ActiveIdentity is Hero))
                {
                    List<ICharacter> enemies = new() { p };
                    enemies.AddRange(p.CardsInPlay.Allies);
                    await IndirectDamageHandler.inst.HandleIndirectDamage(enemies, 3);
                }
                break;
            case 2:
                foreach (var p in TurnManager.Players)
                {
                    List<ICharacter> enemies = new() { p };
                    enemies.AddRange(p.CardsInPlay.Allies);
                    await IndirectDamageHandler.inst.HandleIndirectDamage(enemies, 3);
                }
                break;
            case 3:
                foreach (var p in TurnManager.Players)
                {
                    await DamageSystem.Instance.ApplyDamage(new(p, 4));
                }
                break;
        }
        
    }

    private Task<SchemeAction> CancelScheme(SchemeAction schemeAction)
    {
        RiskyBusiness.Instance.environment.RemoveCounters(1);
        schemeAction = null;
        return Task.FromResult(schemeAction);
    }
    #endregion
}
