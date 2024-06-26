using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Principal;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ModularSelectionPanel : MonoBehaviour
{
    public static ModularSelectionPanel instance;

    [Header("File Paths")]
    [SerializeField] string modularFile;

    [Header("Transforms")]
    [SerializeField] Transform setsContent;
    [SerializeField] Transform cardsContent;

    [Header("Prefabs")]
    [SerializeField] GameObject cardLabelPrefab;
    [SerializeField] GameObject setButtonPrefab;

    [Header("Card Preview")]
    [SerializeField] TMP_Text cardEffectText;
    [SerializeField] TMP_Text cardTypeText;
    [SerializeField] Image cardArt;

    private void Awake()
    {
        instance ??= this;

        foreach (string l in File.ReadAllLines(modularFile))
        {
            var btn = Instantiate(setButtonPrefab, setsContent);
            btn.GetComponentInChildren<TMP_Text>().text = l;
            btn.GetComponent<ModularSetButton>().LoadData(l);
        }
    }

    public void LoadCardLabels(List<string> ids)
    {
        foreach (Transform child in cardsContent)
            Destroy(child.gameObject);

        foreach (string l in ids)
        {
            var data = Database.GetCardDataById(l);
            var lbl = Instantiate(cardLabelPrefab, cardsContent);
            lbl.GetComponentInChildren<TMP_Text>().text = data.cardName;

            //lbl.GetComponent<ModularCardLabel>().Data = data;
        }
    }

    public void PreviewCard(EncounterCardData data)
    {

    }
}
