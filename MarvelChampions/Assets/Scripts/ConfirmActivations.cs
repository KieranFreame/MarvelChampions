using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmActivations : MonoBehaviour
{
    public void GetPlayerResponses(List<IEffect> responses, ICard trigger)
    {
        List<ICard> activated = new();

        //UI Prompt; if false, return activated;

        //List<ICard> activated = await TargetSystem(responses);

        //foreach (var card in activated)
        //PlayCardSystem.InitiatePlayCard(card);
    }
}
