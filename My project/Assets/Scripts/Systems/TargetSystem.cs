using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class TargetSystem : MonoBehaviour
{
    #region SingletonPattern
    public static TargetSystem instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        states.Add(new IdleState());
        states.Add(new GetCandidatesState());
        states.Add(new SelectTargetState());

        stateMachine.ChangeState(states[0]);
    }
    #endregion

    #region Pointer
    GraphicRaycaster raycaster;
    PointerEventData pointerEventData;
    EventSystem eventSystem;

    void Start()
    {
        raycaster = GameObject.Find("Board").GetComponent<GraphicRaycaster>();
        eventSystem = GetComponent<EventSystem>();

        CancelButton.OnCancelAction += Cancel;
    }
    #endregion

    #region State Machine
    private readonly StateMachine stateMachine = new();
    private readonly List<IState> states = new();

    public void ChangeState (int index)
    {
        stateMachine.ChangeState(states[index]);
    }
    #endregion

    public ObservableCollection<dynamic> candidates = new();
    private dynamic _target;
    public dynamic prevTarget;
    public Type TargetComponent { get; private set; }
    private bool stopCoroutines = false; 

    public Action Action { get; private set; }

    public static event UnityAction CheckGuard;
    public static event UnityAction CheckPatrolAndCrisis;

    public IEnumerator GetTarget<T>(Action action, Action<T> callback)
    {
        ClearFields(action);

        Action = action;
        TargetComponent = typeof(T);

        ChangeState(1);

        while (stateMachine.currentState != states[0] && !stopCoroutines)
            yield return null;

        callback(_target);
    }

    private void ClearFields(Action action = null)
    {
        if (action?.Requirement.ToLower() == "differenttarget")
            prevTarget = _target;
        else
            prevTarget = null;

        if (_target != null)
            _target = null;

        if (candidates.Count != 0)
            candidates.Clear();

        stopCoroutines = false;
    }

    private void Cancel()
    {
        stopCoroutines = true;
        StopAllCoroutines();
    }

    //For Selecting Components
    public IEnumerator SelectTarget<T>(List<T> candidates, Action<T> callback)
    {
        T comp = default;
        stopCoroutines = false;

        ClearFields();

        while (!stopCoroutines)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                pointerEventData = new PointerEventData(eventSystem)
                {
                    position = Input.mousePosition
                };

                List<RaycastResult> results = new();

                raycaster.Raycast(pointerEventData, results);

                results.Find(x => x.gameObject.TryGetComponent(out comp));

                if (comp != null)
                {
                    if (candidates.Contains(comp))
                    {
                        callback(comp);
                        yield break;
                    }
                }
            }

            yield return null;
        } 
    }
    //For getting IStat classes (Attacker, Thwarter, Schemer, Health, Defender, Recovery)
    public IEnumerator SelectTarget<T>(List<T> candidates, Action<ICharacter> callback) where T : IStat
    {
        ICharacter selection = default;
        stopCoroutines = false;

        ClearFields();

        while (!stopCoroutines)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                pointerEventData = new PointerEventData(eventSystem)
                {
                    position = Input.mousePosition
                };

                List<RaycastResult> results = new();

                raycaster.Raycast(pointerEventData, results);

                results.Find(x => x.gameObject.TryGetComponent(out selection));

                if (selection != null)
                {
                    switch (typeof(T).ToString())
                    {
                        case "Attacker":
                            if ((candidates as List<Attacker>).Contains(selection.CharStats.Attacker))
                            {
                                callback(selection);
                                yield break;
                            }
                            break;
                        case "Thwarter":
                            if ((candidates as List<Thwarter>).Contains(selection.CharStats.Thwarter))
                            {
                                callback(selection);
                                yield break;
                            }
                            break;
                        case "Health":
                            if ((candidates as List<Health>).Contains(selection.CharStats.Health))
                            {
                                callback(selection);
                                yield break;
                            }
                            break;
                        case "Defender":
                            if ((candidates as List<Defender>).Contains(selection.CharStats.Defender))
                            {
                                callback(selection);
                                yield break;
                            }
                            break;
                        case "Recovery":
                            if ((candidates as List<Recovery>).Contains(selection.CharStats.Recovery)){
                                callback(selection);
                                yield break;
                            }
                            break;
                    }

                }
            }

            yield return null;
        }
    }

    #region Properties
    public StateMachine StateMachine
    {
        get { return stateMachine; }
    }
    public List<IState> States
    {
        get { return states; }
    }
    public dynamic Target
    {
        get { return _target; }
        set { _target = value; }
    }
    #endregion

    #region States
    abstract class BaseTargetSystemState : BaseState
    {
        protected TargetSystem owner = instance;
    }

    class GetCandidatesState : BaseTargetSystemState
    {
        public override void Enter()
        {
            if (owner.Action.Targets.Count != 0) {
                foreach (TargetType target in owner.Action.Targets)
                {
                    List<dynamic> candidates = GetCandidates(target);

                    foreach (dynamic candidate in candidates)
                    {
                        if (candidate.GetComponent(owner.TargetComponent) != null)
                            owner.candidates.Add(candidate.GetComponent(owner.TargetComponent));
                    }
                } 
            }

            if (owner.Action is AttackAction)
                CheckGuard?.Invoke();
            else if (owner.Action is ThwartAction)
                CheckPatrolAndCrisis?.Invoke();

            if (owner.candidates.Count > 0 && !owner.Action.Targets.Contains(TargetType.TargetAll))
                owner.ChangeState(2);
            else
                owner.ChangeState(0);
                
        }

        private List<dynamic> GetCandidates(TargetType targetType)
        {
            List<dynamic> candidates = new();

            switch (targetType)
            {
                case TargetType.TargetAlly:
                    candidates.AddRange(TurnManager.instance.CurrPlayer.CardsInPlay.Allies); //only want active allies, exclude allies in hand
                    break;
                case TargetType.TargetVillain:
                    candidates.Add(FindObjectOfType<Villain>());
                    break;
                case TargetType.TargetScheme:
                    candidates.AddRange(FindObjectsOfType<Threat>());
                    break;
                default:
                    break;
            }

            return candidates;
        }
    }

    class SelectTargetState : BaseTargetSystemState
    {
        public override void Enter()
        {
            if (owner.candidates.Count == 1)
            {
                owner.Target = owner.candidates[0].GetComponent(owner.TargetComponent);
                owner.ChangeState(0);
            }

            owner.StartCoroutine(SelectTarget());
        }

        IEnumerator SelectTarget()
        {
            while (owner.Target == null)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    owner.pointerEventData = new PointerEventData(owner.eventSystem)
                    {
                        position = Input.mousePosition
                    };

                    List<RaycastResult> results = new();

                    owner.raycaster.Raycast(owner.pointerEventData, results);

                    foreach (RaycastResult result in results)
                    {
                        if (result.gameObject.TryGetComponent(owner.TargetComponent, out var comp) && owner.candidates.Contains(comp))
                        {
                            owner.Target = comp;
                            owner.ChangeState(0);
                        }
                    }
                }

                yield return null;
            }
        }
    }
    #endregion
}
