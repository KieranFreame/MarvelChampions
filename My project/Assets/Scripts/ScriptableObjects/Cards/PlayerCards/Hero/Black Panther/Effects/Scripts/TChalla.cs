using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

[CreateAssetMenu(fileName = "T'Challa", menuName = "MarvelChampions/Identity Effects/Black Panther/AlterEgo")]
public class TChalla : IdentityEffect
{
    public override void LoadEffect(Player _owner)
    {
        owner = _owner;
    }

    public override async Task Setup()
    {
        List<PlayerCardData> upgrades = owner.Deck.deck.Where(x => x.cardTraits.Contains("Black Panther")).Cast<PlayerCardData>().ToList();

        if (upgrades.Count == 0)
        {
            Debug.Log("Code's fucked or you somehow managed to draw all 4 upgrades in your opening hand");
            return;
        }

        List<PlayerCard> cards = CardViewerUI.inst.EnablePanel(upgrades.Cast<CardData>().ToList()).Cast<PlayerCard>().ToList();

        PlayerCard cardToAdd = await TargetSystem.instance.SelectTarget(cards);

        owner.Deck.deck.Remove(cardToAdd.Data);
        owner.Deck.limbo.Add(cardToAdd.Data);

        cardToAdd.transform.SetParent(GameObject.Find("PlayerHandTransform").transform, false);
        owner.Hand.AddToHand(cardToAdd);

        CardViewerUI.inst.DisablePanel();

        owner.Deck.Shuffle();
    }
}
