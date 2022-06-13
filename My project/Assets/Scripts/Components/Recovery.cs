using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recovery : MonoBehaviour
{
    int _recovery;
    private int baseREC;
    AlterEgoData identity;

    private void Start()
    {
        identity = transform.parent.GetComponent<Player>().identity.alterEgo;
        _recovery = baseREC = identity.baseREC;
    }

    public void Recover()
    {
        var animator = transform.parent.Find("IdentityProfile").GetComponent<Animator>();
        if (animator != null)
            animator.Play("Exhaust");
        GetComponent<Health>().RecoverHealth(_recovery);
    }

    public void Refresh()
    {
        _recovery = baseREC = identity.REC;
    }

    public int BaseREC
    {
        get
        {
            return baseREC;
        }
        set
        {
            baseREC = value;
        }
    }

    public int _Recovery
    {
        get { return _recovery; }
    }
}
