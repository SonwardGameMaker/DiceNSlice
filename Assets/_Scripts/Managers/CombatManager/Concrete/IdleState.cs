using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : CombatState
{
    #region fields
    private List<GameAction> _heroesActionOrder;
    #endregion

    public IdleState(CombatStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void EnterState()
    {
        Debug.Log($"Entering {nameof(IdleState)}");
    }

    public override void ExitState()
    {
        Debug.Log($"Exiting {nameof(IdleState)}");
    }

    #region external interactions
    public override void SelectCharacter(Character character)
    {
        if (character is Hero hero)
        {
            _stateMachine.GetState<AbilitActiveState>().TrySetActiveHero(hero);
            _stateMachine.ChangeState<AbilitActiveState>();
        }
            
    }

    public override void Next()
    {
        _stateMachine.ChangeState<EnemyTurnState>();
    }
    #endregion
}
