using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurnState : CombatState
{
    public EnemyTurnState(CombatStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void EnterState()
    {
        Debug.Log($"Entering {nameof(EnemyTurnState)}");
        Next();
    }

    public override void ExitState()
    {
        Debug.Log($"Exiting {nameof(AbilitActiveState)}");
    }

    #region external interactions
    public override void SelectCharacter(Character character)
    {
        // TODO
    }

    public override void Next()
    {
        _stateMachine.NextTurn();
    }
    #endregion
}
