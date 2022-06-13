using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionFactory : MonoBehaviour
{
    public static ActionFactory instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }


    public Action CreateAction(ActionData data)
    {
        switch (data.actionName)
        {
            case "DamageAction":
                return new DamageAction(data);
            case "ThwartAction":
                return new ThwartAction(data);
            case "ChangeValueAction":
                return new ChangeValueAction(data as ChangeValueData);
            case "ApplyStatusAction":
                return new ApplyStatusAction(data as StatusData);
        }

        return null;
    }
}