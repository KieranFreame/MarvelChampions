using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Stephen Strange", menuName = "MarvelChampions/Identity Effects/Doctor Strange/Alter-Ego")]
public class StephenStrange : IdentityEffect
{
    public override void LoadEffect(Player _owner)
    {
        owner = _owner;
        hasActivated = false;
        TurnManager.OnEndPlayerPhase += Reset;
    }

    public override Task Setup()
    {
        CreateInvocationDeck();
        return Task.CompletedTask;
    }

    public override bool CanActivate()
    {
        return !hasActivated;
    }

    public override void Activate()
    {
        InvocationDeck.Instance.Discard();
        hasActivated = true;
    }

    private void CreateInvocationDeck()
    {
        GameObject invo = Instantiate(Resources.Load<GameObject>("Prefabs/InvocationDeck"), GameObject.Find("PlayerUI").transform);
        invo.transform.SetAsLastSibling();
        invo.name = "InvocationDeck";

        var invocation = InvocationDeck.Instance;
    }
}
