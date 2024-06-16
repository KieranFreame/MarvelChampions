using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AttachmentUI : EncounterCardUI
{
    private AttachmentCard attachment;

    [Header("Attachment UI")]
    [SerializeField] private TMP_Text RedTxt;
    [SerializeField] private TMP_Text BlueTxt;

    protected override void OnEnable()
    {
        base.OnEnable();
        if (attachment == null)
        {
            TryGetComponent(out attachment);
            attachment.SetupComplete += LoadData;
        }
    }

    protected override void LoadData()
    {
        base.LoadData();

        if (attachment.ATKModifier > 0) RedTxt.text = attachment.ATKModifier.ToString();
        else RedTxt.gameObject.SetActive(false);

        if (attachment.THWSCHModifier > 0) BlueTxt.text = attachment.THWSCHModifier.ToString();
        else BlueTxt.gameObject.SetActive(false);
    }
}
