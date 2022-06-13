using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeValueAction : Action
{
    Value valueToChange;
    Operation operation;

    public ChangeValueAction(ChangeValueData data) : base(data)
    {
        valueToChange = data.valueToChange;
        operation = data.operation;
    }

    public override void Execute() { }

    public void Execute(dynamic target = null)
    {
        if (target == null)
            return;

        switch (valueToChange){
            case Value.Damage:
                switch (operation)
                {
                    case Operation.Plus:
                        target.damage += value;
                        break;
                    case Operation.Minus:
                        target.damage -= value;
                        break;
                    case Operation.Replace:
                        target.damage = value;
                        break;
                    default:
                        break;
                }
              break;
            default:
               break;
        }
         
    }
}
