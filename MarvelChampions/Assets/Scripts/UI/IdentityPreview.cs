using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IdentityPreview : MonoBehaviour
{
    [Header("Hero Preview")]
    [SerializeField] Image HeroPortrait;
    [SerializeField] TMP_Text HeroName;
    [SerializeField] TMP_Text HeroEffect;
    [SerializeField] TMP_Text Attack;
    [SerializeField] TMP_Text Thwart;
    [SerializeField] TMP_Text Defence;
    [SerializeField] TMP_Text HeroHandSize;
    [SerializeField] TMP_Text HeroHealth;

    [Header("Alter-Ego Preview")]
    [SerializeField] Image AlterEgoPortrait;
    [SerializeField] TMP_Text AlterEgoName;
    [SerializeField] TMP_Text AlterEgoEffect;
    [SerializeField] TMP_Text Recovery;
    [SerializeField] TMP_Text AEHandSize;
    [SerializeField] TMP_Text AEHealth;

    public void ChangePreview(HeroData h, AlterEgoData a)
    {
        HeroPortrait.sprite = (h != null) ? h.heroArt : null;
        HeroName.text = (h != null) ? h.heroName : "";
        HeroEffect.text = (h != null) ? h.effect.effectDescription : "";
        Attack.text = (h != null) ? h.baseATK.ToString() : "0";
        Thwart.text = (h != null) ? h.baseTHW.ToString() : "0";
        Defence.text = (h != null) ? h.baseDEF.ToString() : "0";
        HeroHandSize.text = (h != null) ? h.baseHandSize.ToString("00") : "00";

        HeroHealth.text = AEHealth.text = (a != null) ? a.baseHP.ToString("00") : "00";
        
        AlterEgoPortrait.sprite = (a != null) ? a.alterEgoArt : null;
        AlterEgoName.text = (a != null) ? a.alterEgoName : "";
        AlterEgoEffect.text = (a != null) ? a.effect.effectDescription : "";
        Recovery.text = (a != null) ? a.baseREC.ToString() : "0";
        AEHandSize.text = (a != null) ? a.baseHandSize.ToString("00") : "00";
    }
}
