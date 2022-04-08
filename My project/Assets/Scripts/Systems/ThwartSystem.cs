using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ThwartSystem : MonoBehaviour
{
    #region Singleton Pattern
    public static ThwartSystem instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }
    #endregion
    #region GameEvents
    [SerializeField]
    private GameEvent thwartInitiated;
    [SerializeField]
    private GameEvent thwartComplete;
    #endregion
    #region Fields
    IThwarter thwarter;
    IScheme target;
    List<string> keywords;
    #endregion
    #region Properties
    public IThwarter Thwarter
    {
        get
        {
            return thwarter;
        }
    }
    public IScheme Target
    {
        get
        {
            return target;
        }
    }
    public List<string> Keywords
    {
        get
        {
            return keywords;
        }
    }
    #endregion

    public void Thwart(ThwartAction thwart)
    {
        Debug.Log("Thwarting");

        thwarter = thwart.thwarter;
        target = thwart.target;
        keywords = thwart.keywords;

        thwartInitiated.Raise();

        (target as MonoBehaviour).gameObject.SendMessage("RemoveThreat", thwarter.thwart, SendMessageOptions.DontRequireReceiver);

        thwartComplete.Raise();

        thwarter = null;
        target = null;
        keywords.Clear();
        return;
    }
}
