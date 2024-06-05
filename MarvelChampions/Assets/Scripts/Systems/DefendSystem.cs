using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Threading.Tasks;
using System.Threading;

public class DefendSystem
{
    private static DefendSystem instance;

    public static DefendSystem Instance
    {
        get
        {
            instance ??= new();
            return instance;
        }
    }

    DefendSystem()
    {
        PauseMenu.OnRestartGame += Restart;
    }

    private void Restart()
    {
        PauseMenu.OnRestartGame -= Restart;
        instance = null;
    }

    #region Events
    public event UnityAction<Player, AttackAction> OnSelectingDefender;
    public event UnityAction<ICharacter, AttackAction> OnDefenderSelected;
    public event UnityAction<ICharacter> OnTargetSelected;
    #endregion

    #region Fields
    public ICharacter Target { get; set; }
    public readonly List<ICharacter> candidates = new();
    public bool Defended { get; private set; }
    #endregion

    #region Methods

    public async Task<ICharacter> GetDefender(Player targetOwner, AttackAction action)
    {
        candidates.Clear();

        Debug.Log("Select Defender");

        candidates.AddRange(targetOwner.CardsInPlay.Allies);

        candidates.RemoveAll(x => x == null);
        candidates.RemoveAll(x => !(x as AllyCard).CanDefend);

        if (targetOwner.Identity.ActiveIdentity is Hero && !targetOwner.Exhausted)
            candidates.Add(targetOwner);

        OnSelectingDefender?.Invoke(targetOwner, action);
        Target = targetOwner;

        if (candidates.Count > 0)
        {
            CancellationToken token = CancelButton.ToggleCancelBtn(true, DefenderSelectionCanceled);

            Target = await TargetSystem.instance.SelectTarget(candidates, token:token);

            CancelButton.ToggleCancelBtn(false, DefenderSelectionCanceled);

            OnDefenderSelected?.Invoke(Target, action);

            Defended = Target == null;
        }
        else
        {
            Debug.Log("No Defenders Available");
            Defended = false;
            Target = null;
        }
        
        if (Target != null)
        {
            if (Target is Player)
            {
                int defence = await Target.CharStats.Defender.Defend();
                AttackSystem.Instance.Action.Value -= defence;
            }
            else
            {
                (Target as AllyCard).Exhaust();
            }
        }

        Target ??= targetOwner;

        OnTargetSelected?.Invoke(Target);

        return Target;
    }

    private void DefenderSelectionCanceled()
    {
        CancelButton.ToggleCancelBtn(false, DefenderSelectionCanceled);
    }
    #endregion
}