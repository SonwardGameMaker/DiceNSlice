using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: later make this class abstract and create derivative classes for every off combat state
public class OffCombatState : CombatState
{
    #region events
    public event Action OnOffCombatStateStarts;
    public event Action OnOffCombatStateEnds;
    #endregion

    public OffCombatState(CombatStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void EnterState()
    {
        OnOffCombatStateStarts?.Invoke();
    }

    public override void ExitState()
    {
        OnOffCombatStateEnds?.Invoke();
    }

    public override void Next()
    {
        _stateMachine.ChangeState<PreparingState>();
    }

    public override void SelectCharacter(Character character)
    {
        
    }
}
