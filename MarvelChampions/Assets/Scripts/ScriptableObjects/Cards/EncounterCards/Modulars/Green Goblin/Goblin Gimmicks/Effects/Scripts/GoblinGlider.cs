using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Goblin Glider", menuName = "MarvelChampions/Card Effects/Modulars/Goblin Gimmicks/Goblin Glider")]
public class GoblinGlider : AttachmentCardEffect
{
    public override Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
    {
        Card = card;

        List<ICharacter> attachable = new() { owner };
        attachable.AddRange(VillainTurnController.instance.MinionsInPlay);
        attachable.RemoveAll(x => x.Attachments.Where(x => (x as MonoBehaviour).gameObject.name == "Goblin Glider").Count() != 0);

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
            ScenarioManager.inst.Surge(player);
            ScenarioManager.inst.EncounterDeck.Discard(card);
        }

        return Task.CompletedTask;
    }

    public override bool CanActivate(Player p)
    {
        return p.HaveResource(Resource.Energy, 2);
    }

    public override async Task Activate(Player p)
    {
        await PayCostSystem.instance.GetResources(Resource.Energy, 2);

        Detach();
        ScenarioManager.inst.EncounterDeck.Discard(Card);
    }

    public override void Attach()
    {
        attached.Attachments.Add(Card as IAttachment);
        attached.CharStats.Attacker.CurrentAttack++;

        if (attached is MinionCard)
        {
            Card.transform.SetParent((attached as MonoBehaviour).transform, false);
            Card.transform.SetAsFirstSibling();
            Card.transform.localPosition = new Vector3(-30, 0, 0);
        }
    }

    public override void Detach()
    {
        attached.Attachments.Remove(Card as IAttachment);
        attached.CharStats.Attacker.CurrentAttack--;
    }
}
