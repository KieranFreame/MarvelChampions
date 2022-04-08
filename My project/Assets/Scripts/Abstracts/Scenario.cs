using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public /*abstract*/ class Scenario
{
    public Villain villain { get; set; }

    public List<MainScheme> mainSchemes = new List<MainScheme>();

    public Deck encounterDeck = new Deck();

    //public string[] encounterSets;

    public List<SideScheme> activeSideSchemes = new List<SideScheme>();
    public List<IEnvironment> activeEnvironments = new List<IEnvironment>();
    public List<Minion> activeMinions = new List<Minion>();

    private void Update()
    {
        if (encounterDeck.Update())
        {
            mainSchemes[0].acceleration++;
        }
    }

    public void Discard(Card encounter)
    {
        if (encounter is Encounter)
        {
            encounterDeck.Discard(encounter);

            if (encounter is SideScheme)
                RemoveSideScheme(encounter as SideScheme);
            else if (encounter is IEnvironment)
                RemoveEnvironment(encounter as IEnvironment);
            else if (encounter is Minion)
                RemoveMinion(encounter as Minion);
            else if (encounter is IAttachment)
                RemoveAttachment(encounter as IAttachment);
        }
        else
        {
            //discard to player discard
            //i.e. Cosmic Entity events
        }
    }

    #region RemoveCardType
    public void RemoveSideScheme(SideScheme sideScheme)
    {
        for (int i = 0; i < activeSideSchemes.Count; i++)
        {
            if (sideScheme == activeSideSchemes[i])
            {
                activeSideSchemes.RemoveAt(i);
                return;
            }
        }
    }
    public void RemoveEnvironment(IEnvironment environment)
    {
        for (int i = 0; i < activeEnvironments.Count; i++)
        {
            if (environment == activeEnvironments[i])
            {
                activeEnvironments.RemoveAt(i);
                return;
            }
        }
    }
    public void RemoveMinion(Minion minion)
    {
        for (int i = 0; i < activeMinions.Count; i++)
        {
            if (minion == activeMinions[i])
            {
                activeMinions.RemoveAt(i);
                return;
            }
        }
    }
    public void RemoveAttachment(IAttachment attachment)
    {
        for (int i = 0; i < villain.attachments.Count; i++)
        {
            if (attachment == villain.attachments)
            {
                villain.attachments.RemoveAt(i);
                return;
            }
        }
    }
    #endregion

    public void AdvanceMainScheme()
    {
        mainSchemes.RemoveAt(0);

        if (mainSchemes.Count == 0)
        {
            //end game;
        }
    }
}
