using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace CaptainAmericaHeroPack
{
    [CreateAssetMenu(fileName = "Agent 13", menuName = "MarvelChampions/Card Effects/Captain America/Agent 13")]
    public class AgentThirteen : PlayerCardEffect
    {
        public override async Task OnEnterPlay()
        {
            if (ScenarioManager.sideSchemes.Count == 0 && ScenarioManager.inst.MainScheme.Threat.CurrentThreat == 0)
                return;

            await ThwartSystem.Instance.InitiateThwart(new(2));
        }

        public override bool CanBePlayed()
        {
            if (base.CanBePlayed())
            {
                //Unique Rule
                return !_owner.CardsInPlay.Allies.Any(x => (x.Data as AllyCardData).alterEgo == "Sharon Carter");
            }

            return false;
        }
    }
}

