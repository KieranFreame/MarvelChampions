using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class Schemer : IConfusable
{
    private int _scheme;
    public int BaseScheme { get; set; }
    private bool _confused;
    public List<string> keywords = new(); //temp?

    public dynamic Owner { get; private set; }

    public event UnityAction<bool> OnToggleConfuse;
    public event UnityAction SchemeChanged;

    public delegate Task<SchemeAction> CancelScheme(SchemeAction schemeAction);
    public List<CancelScheme> SchemeCancel { get; private set; } = new();

    public Schemer(Villain owner)
    {
        Owner = owner;
        CurrentScheme = BaseScheme = owner.BaseScheme;
        owner.Stages.StageAdvanced += AdvanceStage;
    }

    public Schemer(MinionCard owner, MinionCardData data)
    {
        Owner = owner;
        CurrentScheme = BaseScheme = data.baseScheme;
    }

    public async Task<SchemeAction> Scheme()
    {
        if (Confused)
        {
            Confused = false;
            return null;
        }

        var scheme = new SchemeAction(_scheme, Owner);

        for (int i = SchemeCancel.Count-1; i >= 0; i--)
        {
            scheme = await SchemeCancel[i](scheme);

            if (scheme == null)
                break;
        }

        return scheme;
    }

    private void AdvanceStage() => CurrentScheme += Owner.BaseScheme - BaseScheme;

    public bool Confused
    {
        get => _confused;
        set 
        { 
            _confused = value;
            OnToggleConfuse?.Invoke(_confused);
        }
    }

    public int CurrentScheme
    {
        get { return _scheme; }
        set
        {
            _scheme = value;
            SchemeChanged?.Invoke();
        }
    }
}
