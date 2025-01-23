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
        throw new System.NotImplementedException();
    }

    public override void ExitState()
    {
        throw new System.NotImplementedException();
    }

    #region external interactions
    public override void SelectCharacter(Character character)
    {
        // TODO
    }

    public override void NextState()
    {
        // TODO
    }
    #endregion
}
