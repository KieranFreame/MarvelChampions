using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Goblin Glider", menuName = "MarvelChampions/Card Effects/Modulars/Goblin Gimmicks/Goblin Glider")]
public class GoblinGlider : AttachmentCardEffect
{
    public override Task OnEnterPlay()
    {
        List<ICharacter> attachable = new(VillainTurnController.instance.MinionsInPlay) { _owner };
        attachable.RemoveAll(x => x.Attachments.Any(x => ((IEffect)x).Card.CardName == "Goblin Glider"));

        int highHP = int.MinValue;

        foreach (var c in attachable)
        {
            if (c.CharStats.Health.BaseHP > highHP)
            {
                highHP = c.CharStats.Health.BaseHP;
                attached = c;
            }
        }

        if (attached != null)
        {
            Attach();
        }
        else
        {
            ScenarioManager.inst.Surge(TurnManager.instance.CurrPlayer);
            ScenarioManager.inst.EncounterDeck.Discard(_card);
        }

        return Task.CompletedTask;
    }

    public override bool CanActivate(Player p) => p.HaveResource(Resource.Energy, 2);

    public override async Task Activate(Player p)
    {
        await PayCostSystem.instance.GetResources(new() { { Resource.Energy, 2 } });

        Detach();
        ScenarioManager.inst.EncounterDeck.Discard(Card);
    }

    public override void Attach()
    {
        attached.Attachments.Add(Card as IAttachment);
        attached.CharStats.Attacker.CurrentAttack++;

        if (attached is MinionCard)
        {
            _card.transform.SetParent((attached as MonoBehaviour).transform, false);
            _card.transform.SetAsFirstSibling();
            _card.transform.localPosition = new Vector3(-30, 0, 0);
        }
    }

    public override void Detach()
    {
        attached.Attachments.Remove(Card as IAttachment);
        attached.CharStats.Attacker.CurrentAttack--;
    }
}
