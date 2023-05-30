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
        CardData draw = _villain.EncounterDeck.DealCard();
        var inst = Instantiate(PrefabFactory.instance.CreateEncounterCard(draw as EncounterCardData), targetParent);
        inst.name = _villain.EncounterDeck.deck[0].cardName;

        inst.GetComponent<EncounterCard>().LoadCardData(draw, _villain.gameObject);
    }

    public GameObject CreateEncounterCard(EncounterCardData card, bool faceDown)
    {
        GameObject inst = Instantiate(PrefabFactory.instance.CreateEncounterCard(card), _parentTransform);
        inst.name = card.cardName;

        inst.GetComponent<EncounterCard>().LoadCardData(card, _villain.gameObject);

        if (!faceDown)
        {
            inst.GetComponent<Card>().Flip();
        }
        else
        {
            inst.GetComponent<Card>().FaceUp = true;
        }
        return inst;
    }

}
