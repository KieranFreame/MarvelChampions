using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thwarter : MonoBehaviour
{
    public int _thwart { get; set; }
    public List<string> keywords = new List<string>(); //temp?
    public int baseTHW { get; set; }
    public bool confused;
    [SerializeField]
    private bool isPlayer;
    private dynamic data;

    private void Start()
    {
        if (!isPlayer)
        {
            Ally ally = GetComponent<CardUI>().card.data as Ally;
            _thwart = baseTHW = ally.thwarter.baseThwart;
        }
        else
        {
            HeroData hero = transform.parent.GetComponent<Player>().identity.hero;
            _thwart = baseTHW = hero.baseTHW;
        }
    }

    public void Thwart()
    {
        if (GetComponent<CardUI>().card.exhausted)
            return;

        GetComponent<CardUI>().card.exhausted = true;
        var animator = GetComponent<Animator>();
        if (animator != null)
            animator.Play("Exhaust");

        if (confused)
        {
            confused = false;
            return;
        }

        var thwart = new ThwartAction(owner:this);
        thwart.Execute();
    }

    public void SendTrigger()
    {
        if (isPlayer)
            data = transform.parent.GetComponent<Player>().identity.hero;
        else
            data = GetComponent<CardUI>().card.data as Ally;

        if (data.actions[0].trigger == "AfterThwart")
        {
            gameObject.SendMessage("AfterThwart", SendMessageOptions.DontRequireReceiver);
        }        
    }
}
