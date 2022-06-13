using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroUI : MonoBehaviour
{
    #region UIElements
    private HeroData hero;
    public Image heroPortrait;
    public Text heroATK;
    public Text heroTHW;
    public Text heroDEF;
    public Text heroHP;
    #endregion

    private void Awake()
    {
        hero = transform.parent.GetComponent<Player>().identity.hero;
        //heroPortrait.sprite = hero.art;
        heroATK.text = hero.baseATK.ToString();
        heroTHW.text = hero.baseTHW.ToString();
        heroDEF.text = hero.baseDEF.ToString();
    }

    private void OnEnable()
    {
        Refresh();
    }

    public void Refresh()
    {
        //heroPortrait.sprite = hero.art;
        heroATK.text = transform.parent.GetComponent<Attacker>()._attack.ToString();
        heroTHW.text = transform.parent.GetComponent<Thwarter>()._thwart.ToString();
        //heroDEF.text = transform.parent.GetComponent<Attacker>()._Attack.ToString();
    }
}
