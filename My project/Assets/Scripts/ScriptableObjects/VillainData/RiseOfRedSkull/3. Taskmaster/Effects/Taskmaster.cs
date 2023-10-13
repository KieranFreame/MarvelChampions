using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace RiseOfRedSkull
{
    [CreateAssetMenu(fileName = "Taskmaster", menuName = "MarvelChampions/Villain Effects/RotRS/Taskmaster")]
    public class Taskmaster : VillainEffect
    {
        public override void LoadEffect(Villain owner)
        {
            foreach (var p in TurnManager.Players)
            {
                p.Identity.FlippedToHero += DealDamage;
            }
        }

        private void DealDamage(Player p)
        {
            int damage = (ScenarioManager.inst.EncounterDeck.deck[0] as EncounterCardData).boostIcons;
            ScenarioManager.inst.EncounterDeck.Mill(1);

            Debug.Log("Taking " + damage + " damage");

            p.CharStats.Health.TakeDamage(new(p, damage));
        }

        public override Task StageTwoEffect()
        {
            foreach (var p in TurnManager.Players)
            {
                ScenarioManager.inst.Surge(p);
            }

            return Task.CompletedTask;
        }

        public override Task StageThreeEffect()
        {
            foreach (var p in TurnManager.Players)
            {
                ScenarioManager.inst.Surge(p);
            }

            return Task.CompletedTask;
        }
    }
}

