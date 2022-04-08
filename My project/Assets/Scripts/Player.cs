using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Fields
        #region Identity
        public Hero hero;
        public AlterEgo alterEgo;
        public IIdentity activeIdentity;
        #endregion
    
    public Deck deck = new Deck();
    public List<Card> encounterCards = new List<Card>();

    public int allyLimit { get; set; }
    public List<Ally> activeAllies = new List<Ally>();

    public List<Card/*change to Upgrades*/> upgrades = new List<Card>();
    public List<Card/*change to Supports*/> supports = new List<Card>();
    public List<IAttachment> attachments = new List<IAttachment>();

    #endregion

    private void Start()
    {
        activeIdentity = alterEgo;
        allyLimit = 3;
    }

    public void RevealEncounterCards()
    {
        foreach (Card c in encounterCards)
        {
            (c as Encounter).Effect();
            GameManager.instance.scenario.Discard(c);
        }
    }
}
