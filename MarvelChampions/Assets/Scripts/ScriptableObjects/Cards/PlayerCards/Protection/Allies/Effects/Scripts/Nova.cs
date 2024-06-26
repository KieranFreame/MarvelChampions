using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;
using UnityEngine;

namespace MsMarvelHeroPack
{
    [CreateAssetMenu(fileName = "Nova", menuName = "MarvelChampions/Card Effects/Protection/Allies/Nova")]
    public class Nova : PlayerCardEffect
    {
        public override Task OnEnterPlay()
        {
            ScenarioManager.inst.ActiveVillain.CharStats.Attacker.AttackCancel.Add(AttackInitiated);

            foreach (MinionCard m in VillainTurnController.instance.MinionsInPlay)
            {
                m.CharStats.Attacker.AttackCancel.Add(AttackInitiated);
            }

            VillainTurnController.instance.MinionsInPlay.CollectionChanged += MinionsChanged;

            return Task.CompletedTask;
        }

        private void MinionsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (MinionCard m in e.NewItems)
                    {
                        m.CharStats.Attacker.AttackCancel.Add(AttackInitiated);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (MinionCard m in e.OldItems)
                    {
                        m.CharStats.Attacker.AttackCancel.Remove(AttackInitiated);
                    }
                    break;
            }
        }

        private async Task<AttackAction> AttackInitiated(AttackAction action)
        {
            if (_owner.HaveResource(Resource.Energy))
            {
                bool choice = await ConfirmActivateUI.MakeChoice(Card);

                if (choice)
                {
                    await PayCostSystem.instance.GetResources(new() { { Resource.Energy, 1 } });
                    action.Owner.CharStats.Health.TakeDamage(new(action.Owner, 2, card: Card, owner: _owner));

                    if (action.Owner.CharStats.Health.CurrentHealth <= 0)
                    {
                        action = null;
                        return action;
                    }
                        
                }
            }

            return action;
        }

        public override void OnExitPlay()
        {
            ScenarioManager.inst.ActiveVillain.CharStats.Attacker.AttackCancel.Remove(AttackInitiated);

            foreach (MinionCard m in VillainTurnController.instance.MinionsInPlay)
            {
                m.CharStats.Attacker.AttackCancel.Remove(AttackInitiated);
            }

            VillainTurnController.instance.MinionsInPlay.CollectionChanged -= MinionsChanged;
        }
    }
}

