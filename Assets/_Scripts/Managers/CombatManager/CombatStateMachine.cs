using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CombatStateMachine
{
    #region fields
    // Combat data
    private int _turnCount = 0;

    // State
    private CombatState _currentState;
    private List<CombatState> _stateList;
    #endregion

    #region events
    public event Action OnTurnEnded;
    #endregion

    #region init
    public CombatStateMachine()
    {
        _stateList = new List<CombatState>();
        _stateList.Add(new PreparingState(this));
        _stateList.Add(new RollingSate(this));
        _stateList.Add(new IdleState(this));
        _stateList.Add(new AbilitActiveState(this));
        _stateList.Add(new EnemyTurnState(this));
    }

    public void Setup<T>() where T : CombatState
    {
        _currentState = _stateList.Find(s => s is T) as T;
        _currentState.EnterState();
    }

    ~CombatStateMachine()
    {
        _currentState.ExitState();
    }
    #endregion

    #region properties
    public int TurnCount => _turnCount;
    public CombatState CurrentState => _currentState;
    #endregion

    #region external interactions
    public void ChangeState<T>() where T : CombatState
    {
        _currentState.ExitState();
        _currentState = _stateList.Find(s => s as T is T) as T;
        _currentState.EnterState();
    }

    public T GetState<T>() where T : CombatState
        => _stateList.Find(s => s is T) as T;

    public void NextTurn()
    {
        _turnCount++;
        ChangeState<PreparingState>();
        OnTurnEnded?.Invoke();
    }
    #endregion
}
