using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Ancestral Knowledge", menuName = "MarvelChampions/Card Effects/Black Panther/Ancestral Knowledge")]
public class AncestralKnowledge : PlayerCardEffect
{
    public override bool CanBePlayed()
    {
        if (base.CanBePlayed())
        {
            if (_owner.Identity.ActiveIdentity is not AlterEgo)
                return false;

            if (_owner.Deck.discardPile.Count == 0)
                return false;

            return true;
        }

        return false;
    }

    public override async Task OnEnterPlay()
    {
        List<PlayerCardData> discardPile = _owner.Deck.discardPile.Cast<PlayerCardData>().ToList();
        List<PlayerCard> discards = CardViewerUI.inst.EnablePanel(discardPile.Cast<CardData>().ToList()).Cast<PlayerCard>().ToList();

        CancellationToken token = FinishButton.ToggleFinishButton(true, FinishedSelection);
        List<PlayerCard> shuffleBack = await TargetSystem.instance.SelectTargets(discards, 3, token);
        FinishButton.ToggleFinishButton(false, FinishedSelection);

        List<CardData> dataToShuffle = new();

        for (int i = shuffleBack.Count-1; i >= 0; i--)
        {
            CardData data = _owner.Deck.discardPile.First(x => x == shuffleBack[i].Data);
            _owner.Deck.discardPile.Remove(data);
            dataToShuffle.Add(data);
        }

        CardViewerUI.inst.DisablePanel();
        _owner.Deck.AddToDeck(dataToShuffle);
    }

    private void FinishedSelection()
    {
        FinishButton.ToggleFinishButton(false, FinishedSelection);
    }
}
