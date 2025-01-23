using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : CombatState
{
    #region fields
    private List<GameAction> _heroesActionOrder;

    // temp
    private Hero _activeHero;
    #endregion

    #region events
    public event Action<Hero> OnHeroActivated;
    public event Action<Hero> OnHeroDeactivated;
    #endregion

    public IdleState(CombatStateMachine stateMachine) : base(stateMachine)
    {
    }

    #region properties
    public Hero ActiveHero { get => _activeHero; }
    #endregion

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
        _stateMachine.ChangeState<EnemyTurnState>();
    }
    #endregion
}
