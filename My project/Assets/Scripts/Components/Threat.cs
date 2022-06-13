using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Threat : MonoBehaviour
{
    int _thwart;
    private int startingThreat;
    public int currThreat;

    [SerializeField]
    private GameEvent beingThwarted;

    private void Start()
    {
        var data = GetComponent<CardUI>().card.data as Scheme;
        currThreat = startingThreat = data.startingThreat;
    }

    public void RemoveThreat(int thwart)
    {
        _thwart = thwart;
        //beingThwarted.Raise();

        if (_thwart > 0)
        {
            currThreat -= _thwart;

            if (currThreat <= 0)
            {
                gameObject.SendMessage("WhenDefeated", SendMessageOptions.DontRequireReceiver);
            }
        }

        GetComponent<CardUI>().Refresh();
    }

    public void GainThreat(int threat)
    {
        currThreat += threat;
    }

    public int StartingThreat
    {
        get
        {
            return startingThreat;
        }
        set
        {
            startingThreat = value;
        }
    }

    public int Damage
    {
        get { return _thwart; }
    }
}
