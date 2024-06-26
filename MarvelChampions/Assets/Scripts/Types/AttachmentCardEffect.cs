using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class AttachmentCardEffect : EncounterCardEffect
{
    public ICharacter attached { get; protected set; }

    public override Task OnEnterPlay()
    {
        Attach();
        return Task.CompletedTask;
    }

    public virtual void Attach()
    {
        _card.transform.SetParent((attached as MonoBehaviour).transform);
        _card.transform.SetAsFirstSibling();
        _card.transform.localPosition = new Vector3(-30 * attached.Attachments.Count, 0, 0);
    }

    public virtual void Attach(ICharacter _attached, AttachmentCard card)
    {
        Card = card;
        attached = _attached;
        Attach();
    }
    public virtual void Detach() { }
    public void Reattach(ICharacter newAttached)
    {
        Detach();
        attached = newAttached;
        Attach();
    }
    public virtual void WhenRemoved()
    {
        Detach();
        ScenarioManager.inst.EncounterDeck.Discard(Card);
    }
}
