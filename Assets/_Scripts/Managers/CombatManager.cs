using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    CombatStateMachine _stateMachine;

    #region events
    public event Action OnTurnEnded;

    public event Action<Hero> OnHeroActivated;
    public event Action<Hero> OnHeroDeactivated;
    #endregion

    #region init
    public void Setup()
    {
        _stateMachine = new CombatStateMachine();

        _stateMachine.OnTurnEnded += OnTurnEndedHandler;

        SubscribeToAbilityActiveState();

        void SubscribeToAbilityActiveState()
        {
            AbilitActiveState abilitActiveState = _stateMachine.GetState<AbilitActiveState>();
            if (abilitActiveState == null) throw new NullReferenceException(nameof(AbilitActiveState));

            abilitActiveState.OnHeroActivated += OnHeroActivatedHandlder;
            abilitActiveState.OnHeroDeactivated += OnHeroDeactivatedHandlder;


        }
    }

    private void OnDestroy()
    {
        UnsubscribeToAbilityActiveState();

        _stateMachine.OnTurnEnded -= OnTurnEndedHandler;

        void UnsubscribeToAbilityActiveState()
        {
            AbilitActiveState abilitActiveState = _stateMachine.GetState<AbilitActiveState>();

            abilitActiveState.OnHeroActivated -= OnHeroActivated;
            abilitActiveState.OnHeroDeactivated -= OnHeroDeactivated;
        }
    }
    #endregion

    #region properties
    public CombatStateMachine StateMachine => _stateMachine;
    #endregion

    #region external interactions
    public void StartCombat()
        => _stateMachine.Setup<PreparingState>();

    public void SelectCharacter(Character character)
        => _stateMachine.CurrentState.SelectCharacter(character);

    public void Next()
        => _stateMachine.CurrentState.Next();

    public void Cancel()
    {
        if (_stateMachine.CurrentState is AbilitActiveState abilityActiveState)
            abilityActiveState.Next();
    }
    #endregion

    #region event handlers
    private void OnTurnEndedHandler()
        => OnTurnEnded?.Invoke();

    private void OnHeroActivatedHandlder(Hero hero)
        => OnHeroActivated?.Invoke(hero);

    private void OnHeroDeactivatedHandlder(Hero hero)
        => OnHeroDeactivated?.Invoke(hero);
    #endregion
}
