using System.Collections.Specialized;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System;
using System.Collections.ObjectModel;

[CreateAssetMenu(fileName = "CrisisInterdiction", menuName = "MarvelChampions/Card Effects/Captain Marvel/Crisis Interdiction")]
public class CrisisInterdiction : PlayerCardEffect
{
    List<SchemeCard> schemes;

    public override bool CanBePlayed()
    {
        return base.CanBePlayed() && ScenarioManager.inst.ThreatPresent();
    }

    public override async Task OnEnterPlay()
    {
        if (_owner.Identity.IdentityTraits.Contains("Aerial"))
            GameStateManager.Instance.OnActivationCompleted += IsTriggerMet;

        await _owner.CharStats.InitiateThwart(new(2, Owner));
    }

    private void IsTriggerMet(Action action)
    {
        if (action is not ThwartAction || ((ThwartAction)action).Owner != Owner) { return; }

        var thwart = (ThwartAction)action;
        GameStateManager.Instance.OnActivationCompleted -= IsTriggerMet;

        if (!ScenarioManager.inst.ThreatPresent())
            return;

        schemes = new(ScenarioManager.sideSchemes.Where(x => x != thwart.Target));
        if (thwart.Target != ScenarioManager.inst.MainScheme && ThwartSystem.Instance.Crisis.Count == 0)
            schemes.Add(ScenarioManager.inst.MainScheme);

        if (schemes.Count == 0) return;

        EffectManager.Inst.Resolving.Push(this);
    }

    public override async Task Resolve()
    {
        Debug.Log("Captain Marvel has the Aerial trait. Please select an additional scheme to thwart");
        var target = await TargetSystem.instance.SelectTarget(schemes);
        target.Threat.RemoveThreat(2);
    }
}
