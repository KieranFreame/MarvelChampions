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

    public ObservableCollection<dynamic> candidates = new();
    //private dynamic Target;
    private bool stopCoroutines = false; 

    public Action Action { get; private set; }

    public static event UnityAction<List<ICharacter>> CheckGuard;
    public static event UnityAction<List<Threat>> CheckPatrolAndCrisis;

    private void ClearFields()
    {
        candidates.Clear();
        stopCoroutines = false;
    }

    private void Cancel()
    {
        stopCoroutines = true;
        StopAllCoroutines();
    }

    //For Selecting Components
    public IEnumerator SelectTarget<T>(List<T> candidates, Action<T> callback, bool isAttack = false)
    {
        T comp = default;

        ClearFields();

        if (isAttack)
            CheckGuard?.Invoke(candidates as List<ICharacter>);

        if (typeof(T).ToString() is "Threat")
            CheckPatrolAndCrisis?.Invoke(candidates as List<Threat>);

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
}
