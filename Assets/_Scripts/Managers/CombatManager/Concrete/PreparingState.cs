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
    }

    public override void ExitState()
    {
        
    }
    #endregion

    #region external interactions
    public override void SelectCharacter(Character character)
    {
        // TODO
    }

    public override void NextState()
    {
        _stateMachine.ChangeState<RollingSate>();
    }
    #endregion
}
