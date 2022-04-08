using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThwartAction
{
    public IThwarter thwarter;
    public IScheme target;
    public List<string> keywords;

    public ThwartAction(IThwarter thwarter, IScheme target, List<string> keywords)
    {
        this.thwarter = thwarter;
        this.target = target;
        this.keywords = keywords;
    }
}
