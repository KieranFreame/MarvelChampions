using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Alter-Ego", menuName = "Identity/Alter-Ego")]
public class AlterEgoData : ScriptableObject
{
    [Header("CardDetails")]
    public string alterEgoName;
    public List<string> alterEgoTags;
    public Sprite alterEgoArt;

    [Header("Stats")]
    public int baseREC;
    public int baseHP; //doesn't change between hero & alterego, so store on alter-ego only;
    public int baseHandSize;

    [Header("Ability")]
    public IdentityEffect effect;

    //[Header("Identity Cards")]
    //public CardSet heroCards;
    //public CardSet nemesisCards;
}
