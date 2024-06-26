using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Sonic Boom", menuName = "MarvelChampions/Card Effects/Klaw/Sonic Boom")]
public class SonicBoom : EncounterCardEffect
{
    readonly Dictionary<Resource, int> cost = new()
    {
        {Resource.Energy, 1 },
        {Resource.Scientific, 1 },
        {Resource.Physical, 1 },
    };

    public override async Task Resolve()
    {
        var player = TurnManager.instance.CurrPlayer;

        if (player.HaveResource(Resource.Energy)  && player.HaveResource(Resource.Scientific) && player.HaveResource(Resource.Physical))
        {
            int decision = await ChooseEffectUI.ChooseEffect(new List<string>() { "Spend one of each resource (Energy, Scientific and Physical)", "Exhaust each character you control" });

            if (decision == 1)
            {
                await PayCostSystem.instance.GetResources(cost);
                return;
            }
        }

        player.Exhaust();
        foreach (var a in player.CardsInPlay.Allies)
        {
            a.Exhaust();
        }
    }

    Player target;

    public override Task Boost(Action action)
    {
        if (action is AttackAction)
        {
            var attack = action as AttackAction;
            if (attack.Target is Player)
            {
                target = attack.Target as Player;
                target.CharStats.Health.OnTakeDamage += OnTakeDamage;
            }
        }

        return Task.CompletedTask;
    }

    private void OnTakeDamage(DamageAction action)
    {
        if (action.Value > 0)
        {
            target.Exhaust();
        }

        target.CharStats.Health.OnTakeDamage -= OnTakeDamage;
    }
}
