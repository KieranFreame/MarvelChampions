using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Whirlwind", menuName = "MarvelChampions/Card Effects/Masters of Evil/Whirlwind")]
public class Whirlwind : EncounterCardEffect
{
    public override Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
    {
        _owner = owner;
        Card = card;

        (Card as MinionCard).CharStats.AttackInitiated += AttackInitiated;
        
        return Task.CompletedTask;
    }

    private void AttackInitiated() => AttackSystem.Instance.OnAttackCompleted.Add(AttackCompleted);
        

    private async Task AttackCompleted(AttackAction action)
    {
        AttackSystem.Instance.OnAttackCompleted.Remove(AttackCompleted);

        List<Player> players;
        if (action.Target is not Player)
            players = TurnManager.Players.Where(x => x != (action.Target as AllyCard).Owner).ToList();
        else
            players = TurnManager.Players.Where(x => x != action.Target as Player).ToList();

        (Card as MinionCard).CharStats.AttackInitiated -= AttackInitiated; //prevent an infinite loop

        foreach (Player player in players)
        {
            action.Target = player;
            await (Card as MinionCard).CharStats.InitiateAttack(action);
        }

        (Card as MinionCard).CharStats.AttackInitiated += AttackInitiated;
    }

    public override async Task Boost(Action action)
    {
        foreach (Player p in TurnManager.Players.Where(x => x.Identity.ActiveIdentity is Hero))
        {
            await DamageSystem.Instance.ApplyDamage(new(p, 1, card:GameObject.Find("Whirlwind").GetComponent<EncounterCard>(), owner:action.Owner));
        }
    }

    public override Task WhenDefeated()
    {
        (Card as MinionCard).CharStats.AttackInitiated += AttackInitiated;

        return Task.CompletedTask;
    }
}
