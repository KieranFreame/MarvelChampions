using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlterEgoUI : MonoBehaviour
{
    #region UIElements
    public Image alteregoPortrait;
    public Text alteregoREC;
    private AlterEgoData alterego;
    #endregion

    private void Start()
    {
        alterego = transform.parent.GetComponent<Player>().identity.alterEgo;
        //alteregoPortrait.sprite = alterego.art;
        alteregoREC.text = alterego.baseREC.ToString();
    }

    public void Refresh()
    {
        alteregoREC.text = transform.parent.GetComponent<Recovery>()._Recovery.ToString();
    }
}
