using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Masters of Mayhem", menuName = "MarvelChampions/Card Effects/Masters of Evil/Masters of Mayhem")]
public class MastersOfMayhem : EncounterCardEffect
{
    public override async Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
    {
        List<MinionCard> masters = VillainTurnController.instance.MinionsInPlay.Where(x => x.CardTraits.Contains("Masters of Evil")).ToList();

        if (masters.Count > 0)
        {
            foreach (var m in masters)
            {
                await m.CharStats.InitiateAttack();
            }
        }
        else
        {
            MinionCardData master = ScenarioManager.inst.EncounterDeck.deck.FirstOrDefault(x => x.cardTraits.Contains("Masters of Evil")) as MinionCardData;

            if (master == default)
            {
                master = ScenarioManager.inst.EncounterDeck.discardPile.FirstOrDefault(x => x.cardTraits.Contains("Masters of Evil")) as MinionCardData;

                if (master == default)
                {
                    Debug.Log("All Masters of Evil have been removed from the game. Surging");
                    ScenarioManager.inst.Surge(player);
                    return;
                }

                ScenarioManager.inst.EncounterDeck.discardPile.Remove(master);
            }
            else
                ScenarioManager.inst.EncounterDeck.deck.Remove(master);

            ScenarioManager.inst.EncounterDeck.limbo.Add(master);

            MinionCard masterofevil = CreateCardFactory.Instance.CreateCard(master, GameObject.Find("MinionTransform").transform) as MinionCard;
            VillainTurnController.instance.MinionsInPlay.Add(masterofevil);
            await masterofevil.Effect.OnEnterPlay(owner, masterofevil, player);
        }
    }
}
