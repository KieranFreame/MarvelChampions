using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevealCardSystem : MonoBehaviour
{
    #region Singleton Pattern
    public static RevealCardSystem instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        _villain = FindObjectOfType<Villain>();
    }
    #endregion

    #region Fields
    private Villain _villain;
    [SerializeField] Transform _parentTransform;
    #endregion

    public void DealEncounterCard(Transform targetParent)
    {
        CardData draw = ScenarioManager.inst.EncounterDeck.DealCard();
        var inst = Instantiate(PrefabFactory.instance.CreateEncounterCard(draw as EncounterCardData), targetParent);
        inst.name = ScenarioManager.inst.EncounterDeck.deck[0].cardName;

        inst.GetComponent<EncounterCard>().LoadCardData(draw as EncounterCardData, _villain);
    }

    public GameObject CreateEncounterCard(EncounterCardData card, bool faceDown)
    {
        EncounterCard inst = Instantiate(PrefabFactory.instance.CreateEncounterCard(card), _parentTransform).GetComponent<EncounterCard>();
        inst.gameObject.name = card.cardName;

        inst.LoadCardData(card, _villain);

        if (!faceDown)
        {
            inst.Flip();
        }
        else
        {
            inst.FaceUp = true;
        }
        return inst.gameObject;
    }

}
