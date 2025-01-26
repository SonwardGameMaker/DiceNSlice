using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO Class
public class PreparingState : CombatState
{
    #region events
    public event Action OnStateStarts;
    #endregion

    #region init
    public PreparingState(CombatStateMachine stateMachine) : base(stateMachine)
    {
    }
    #endregion

    #region state controll
    public override void EnterState()
    {
        OnStateStarts?.Invoke();

        Debug.Log($"Entering {nameof(PreparingState)}");
    }

    public override void ExitState()
    {
        Debug.Log($"Exiting {nameof(PreparingState)}");
    }
    #endregion

    #region external interactions
    public override void SelectCharacter(Character character)
    {
        // TODO
    }

    public override void Next()
    {
        _stateMachine.ChangeState<RollingSate>();
    }
    #endregion
}
