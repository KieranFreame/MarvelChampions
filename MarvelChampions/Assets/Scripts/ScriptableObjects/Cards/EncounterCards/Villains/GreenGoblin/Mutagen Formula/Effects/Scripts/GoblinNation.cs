using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Goblin Nation", menuName = "MarvelChampions/Card Effects/Mutagen Formula/Goblin Nation")]
public class GoblinNation : EncounterCardEffect
{
    public override Task OnEnterPlay()
    {
        VillainTurnController.instance.MinionsInPlay.CollectionChanged += AttackModify;

        foreach (var goblin in VillainTurnController.instance.MinionsInPlay.Where(x => x.CardTraits.Contains("Goblin")))
        {
            goblin.CharStats.Attacker.CurrentAttack++;
        }

        _owner.CharStats.Attacker.CurrentAttack++;

        ((SchemeCard)_card).Threat.CurrentThreat *= TurnManager.Players.Count;

        return Task.CompletedTask;
    }

    private void AttackModify(object sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == NotifyCollectionChangedAction.Add)
        {
            foreach (var item in e.NewItems)
            {
                if ((item as MinionCard).CardTraits.Contains("Goblin"))
                {
                    (item as MinionCard).CharStats.Attacker.CurrentAttack++;
                }
            }
        }
        else if (e.Action == NotifyCollectionChangedAction.Remove)
        {
            foreach (var item in e.OldItems)
            {
                if ((item as MinionCard).CardTraits.Contains("Goblin"))
                {
                    (item as MinionCard).CharStats.Attacker.CurrentAttack--;
                }
            }
        }
    }

    public override Task WhenDefeated()
    {
        foreach (var goblin in VillainTurnController.instance.MinionsInPlay.Where(x => x.CardTraits.Contains("Goblin")))
        {
            goblin.CharStats.Attacker.CurrentAttack--;
        }

        _owner.CharStats.Attacker.CurrentAttack--;

        return Task.CompletedTask;
    }

    public override async Task Boost(Action action)
    {
        var card = GameObject.Find("Goblin Nation").GetComponent<EncounterCard>();

        card.InPlay = true;
        card.transform.SetParent(GameObject.Find("SideSchemeTransform").transform);
        ScenarioManager.sideSchemes.Add(card as SchemeCard);
        await card.Effect.OnEnterPlay();
    }
}
