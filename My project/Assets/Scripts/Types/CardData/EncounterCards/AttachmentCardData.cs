using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Attachment", menuName = "MarvelChampions/Card/Attachment")]
public class AttachmentCardData : EncounterCardData
{
    [Header("Attachment Card")]
    public int ATKModifier;
    public int THWSCHModifier;
}
