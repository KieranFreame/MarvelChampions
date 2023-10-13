using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Threading.Tasks;
using System.Threading;

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
    }
    #endregion

    public ObservableCollection<dynamic> candidates = new();
    //private dynamic Target; 

    public Action Action { get; private set; }

    public static event UnityAction<dynamic> TargetAcquired; 
    
    public async Task<T> SelectTarget<T>(List<T> candidates, CancellationToken token = default)
    {
        T comp = default;

        this.candidates.Clear();

        while (!token.IsCancellationRequested)
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
                    if (candidates.Contains(comp))
                        break;
            }

            await Task.Yield();
        }

        TargetAcquired?.Invoke(comp);
        return comp;
    }

    public async Task<List<T>> SelectTargets<T>(List<T> candidates, int amount, CancellationToken token)
    {
        List<T> selections = new();

        while (!token.IsCancellationRequested)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                pointerEventData = new PointerEventData(eventSystem)
                {
                    position = Input.mousePosition
                };

                List<RaycastResult> results = new();

                raycaster.Raycast(pointerEventData, results);

                foreach (RaycastResult r in results)
                {
                    if (r.gameObject.GetComponent<T>() != null)
                    {
                        T component = r.gameObject.GetComponent<T>();

                        if (candidates.Contains(component))
                        {
                            if (!selections.Contains(component))
                                selections.Add(component);
                            else
                                selections.Remove(component);
                        }    
                    }
                }
            }

            if (selections.Count == amount)
                break;

            await Task.Yield();
        }

        foreach (T selection in selections)
            TargetAcquired?.Invoke(selection);

        return selections;
    }
}
