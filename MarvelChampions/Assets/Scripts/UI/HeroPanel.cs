using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroPanel : MonoBehaviour
{
    [SerializeField] Image heroPortrait;
    [SerializeField] TMP_Text heroName;

    public void UpdateIdentity(HeroData arg0)
    {
        heroPortrait.sprite = (arg0 != null) ? arg0.heroArt : null;
        heroName.text = (arg0 != null) ? arg0.heroName : "???";
    }
}
