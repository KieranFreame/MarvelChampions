using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Make the Call", menuName = "MarvelChampions/Card Effects/Leadership/Make the Call")]
public class MakeTheCall : PlayerCardEffect
{
    readonly List<CardData> allies = new();

    public override bool CanBePlayed()
    {
        if( base.CanBePlayed())
        {
            allies.Clear();

            allies.AddRange(_owner.Deck.discardPile.Where(x => x is AllyCardData));

            allies.RemoveAll(x => (x as PlayerCardData).cardCost > _owner.ResourcesAvailable(Card));

            return allies.Count > 0;
        }
        
        return false;
    }

    public override async Task OnEnterPlay()
    {
        List<PlayerCard> cards = CardViewerUI.inst.EnablePanel(allies).Cast<PlayerCard>().ToList();
        var ally = await TargetSystem.instance.SelectTarget(cards);

        ally.transform.SetParent(GameObject.Find("EncounterCards").transform, false);
        ally.transform.localPosition = Vector3.zero;

        CardViewerUI.inst.DisablePanel();

        _owner.Deck.limbo.Add(ally.Data);
        _owner.Deck.discardPile.Remove(ally.Data);

        await PlayCardSystem.Instance.InitiatePlayCard(new(ally));

        _owner.Deck.Discard(Card);
    }
}
