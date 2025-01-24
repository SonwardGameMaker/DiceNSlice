using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingSate : CombatState
{
    public RollingSate(CombatStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void EnterState()
    {
        Debug.Log($"Entering {nameof(RollingSate)}");
        Next();
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
