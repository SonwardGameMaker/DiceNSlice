using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IdleState : CombatState
{
    #region fields
    private List<GameAction> _heroesActionOrder;
    private ICombatCharacterLists _characterLists;
    #endregion

    #region events
    public event Action<Hero> OnHeroActivated;
    #endregion

    public IdleState(CombatStateMachine stateMachine, ICombatCharacterLists characterLists) : base(stateMachine)
    {
        _characterLists = characterLists;
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
        if (character is Hero hero && hero.Dice.RolledSide.Enabled)
        {
            AbilitActiveState abilitActiveState = _stateMachine.GetState<AbilitActiveState>();
            Dice dice = hero.GetComponent<Dice>();
            abilitActiveState.SetActiveHero(hero, dice, dice.RolledSide.GameAction.GetValidTargets
                (_characterLists.PresentHeroes.Select(e => e as Character).ToList(), _characterLists.PresentEnemies.Select(e => e as Character).ToList()));
            _stateMachine.ChangeState<AbilitActiveState>();
            OnHeroActivated?.Invoke(hero);
        }       
    }

    public override void Next()
    {
        _stateMachine.ChangeState<EnemyTurnState>();
    }
    #endregion
}
