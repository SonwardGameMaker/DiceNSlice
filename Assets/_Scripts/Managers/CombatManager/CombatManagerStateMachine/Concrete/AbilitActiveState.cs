using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilitActiveState : CombatState
{
    #region fields
    private Hero _activeHero;
    private Dice _activeHeroDice;
    private List<Character> _validTargets;
    #endregion

    #region events
    public event Action<string> OnWrongActionPerformed;
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
        //ActivateHero();
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
        if (_activeHero != null && _activeHeroDice != null && _validTargets != null && _validTargets.Count != 0)
        {
            if (!_validTargets.Contains(character))
            {
                OnWrongActionPerformed?.Invoke("Target is not valid");
                return;
            }

            if (_activeHeroDice.RolledSide.GameAction is IChooseTargetAction singleTargetAction)
            {
                singleTargetAction.UseOn(character, _activeHeroDice.RolledSide.CurrentPips);
                Next();
            }
            else throw new Exception("Ѕро, тут того немаЇ бути. “ут клуб лише дл€ с≥нгл");
        }
    }

    public override void Next()
    {
        _stateMachine.ChangeState<IdleState>();
    }

    public void SetActiveHero(Hero hero, Dice dice, List<Character> validTargets)
    {
        if (_activeHero == null || _activeHero != hero)
            _activeHero = hero;

        if (_activeHeroDice == null || _activeHeroDice != dice)
            _activeHeroDice = dice;

        if (_validTargets == null || _validTargets != validTargets)
            _validTargets = validTargets;

        //ActivateHero();
    }
    #endregion

    #region internal operactions
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
