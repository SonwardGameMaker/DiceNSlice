using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CombatState
{
    protected CombatStateMachine _stateMachine;

    public CombatState(CombatStateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }

    #region properties
    public CombatStateMachine StateMachine => _stateMachine;
    #endregion

    #region state controll
    public abstract void EnterState();
    public abstract void ExitState();
    #endregion

    #region external interactions
    public abstract void SelectCharacter(Character character);
    public abstract void Next();
    #endregion
}
