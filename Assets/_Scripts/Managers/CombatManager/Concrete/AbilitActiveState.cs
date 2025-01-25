using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilitActiveState : CombatState
{
    #region fields
    private Hero _activeHero;
    #endregion

    #region events
    public event Action<Hero> OnHeroActivated;
    public event Action<Hero> OnHeroDeactivated;
    #endregion

    #region init
    public AbilitActiveState(CombatStateMachine stateMachine) : base(stateMachine)
    {
    }
    #endregion

    #region properties
    public Hero ActiveHero { get => _activeHero; }
    #endregion

    #region state controll
    public override void EnterState()
    {
        Debug.Log($"Entering {nameof(AbilitActiveState)}");
        ActivateHero();
    }

    public override void ExitState()
    {
        Debug.Log($"Exiting {nameof(AbilitActiveState)}");
        DeactivateHero();
    }
    #endregion

    #region external interactions
    public override void SelectCharacter(Character character)
    {
        // TODO
    }

    public override void Next()
    {
        _stateMachine.ChangeState<IdleState>();
    }

    public void SetActiveHero(Hero hero)
    {
        if (_activeHero == null || _activeHero != hero)
            _activeHero = hero;
    }
    #endregion

    #region internal operactions
    private void ActivateHero()
    {
        if (_activeHero != null)
        {
            OnHeroActivated?.Invoke(_activeHero);
        }
    }

    private void DeactivateHero()
    {
        if (_activeHero != null)
        {
            OnHeroDeactivated?.Invoke(_activeHero);
            _activeHero = null;
        }
    }
    #endregion
}
