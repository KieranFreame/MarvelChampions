using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Hail Hydra!", menuName = "MarvelChampions/Card Effects/Hydra/Hail Hydra!")]
public class HailHydra : EncounterCardEffect
{
    public override async Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
    {
        List<MinionCard> hydra = VillainTurnController.instance.MinionsInPlay.Where(x => x.CardTraits.Contains("Hydra")).ToList();

        if (hydra.Count > 0 )
        {
            List<EncounterCardData> data = new();
            data.AddRange(ScenarioManager.inst.EncounterDeck.deck.Where(x => x is MinionCardData && x.cardTraits.Contains("Hydra")).Cast<EncounterCardData>().ToList());
            data.AddRange(ScenarioManager.inst.EncounterDeck.discardPile.Where(x => x is MinionCardData && x.cardTraits.Contains("Hydra")).Cast<EncounterCardData>().ToList());

            hydra = CardViewerUI.inst.EnablePanel(data.Cast<CardData>().ToList()).Cast<MinionCard>().ToList();
            MinionCard hydraMin = await TargetSystem.instance.SelectTarget(hydra);

            VillainTurnController.instance.MinionsInPlay.Add(hydraMin);
            hydraMin.transform.SetParent(GameObject.Find("MinionTransform").transform);
            CardViewerUI.inst.DisablePanel();
            await hydraMin.Effect.OnEnterPlay(owner, hydraMin, player);            
        }
        else
        {
            foreach (var m in hydra)
            {
                await m.CharStats.InitiateAttack();
            }
        }
    }
}
