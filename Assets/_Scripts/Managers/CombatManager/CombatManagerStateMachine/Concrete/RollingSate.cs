using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingSate : CombatState
{
    #region events
    public event Action OnStateStarts;
    #endregion

    public RollingSate(CombatStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void EnterState()
    {
        Debug.Log($"Entering {nameof(RollingSate)}");

        OnStateStarts?.Invoke();
    }

    public override void ExitState()
    {
        Debug.Log($"Exiting {nameof(RollingSate)}");
    }

    #region external interactions
    public override void SelectCharacter(Character character)
    {
        // TODO
    }

    public override void Next()
    {
        _stateMachine.ChangeState<IdleState>();
    }
    #endregion
}
