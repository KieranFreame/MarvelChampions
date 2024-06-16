using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameStateManager
{
    private static GameStateManager instance;

    public static GameStateManager Instance
    {
        get
        {
            instance ??= new();
            return instance;
        }
    }

    public GameState CurrentState { get; set; }

    #region Events
    public UnityAction<ICharacter> OnCharacterDefeated;
    public UnityAction<Action> OnActivationCompleted;
    #endregion

    public void CharacterDefeated(ICharacter defeated) => OnCharacterDefeated?.Invoke(defeated);
    public void ActivationCompleted(Action action) => OnActivationCompleted?.Invoke(action);
}
