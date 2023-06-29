using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Kree Manipulator", menuName = "MarvelChampions/Card Effects/Nemesis/Kree Manipulator")]
public class KreeManipulator : EncounterCardEffect
{
    public override async Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
    {
        _owner = owner;
        Card = card;

        _owner.Surge(player);

        FindObjectOfType<MainSchemeCard>().GetComponent<Threat>().GainThreat(1);

        await Task.Yield();
    }
}
