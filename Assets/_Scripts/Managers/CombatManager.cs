using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    #region fields
    private CombatStateMachine _stateMachine;
    private CombatManagerCharacterController _characterController;
    #endregion

    #region events
    // States
    public event Action OnPreparingStateStarts;
    public event Action OnRollingStateStarts;

    // Characters
    public event Action<Hero> OnHeroActivated;
    public event Action<Hero> OnHeroDeactivated;

    public event Action<Character> OnCharacterEnterScene;
    public event Action<Character> OnCharacterLeaveScene;

    // Combat flow
    public event Action OnTurnEnded;
    #endregion

    #region init
    public void Setup()
    {
        _stateMachine = new CombatStateMachine();

        _characterController = new CombatManagerCharacterController();

        _stateMachine.OnTurnEnded += OnTurnEndedHandler;

        Subscribe();
    }

    private void Setup(List<Hero> heroes, List<Enemy> enemies)
    {
        _stateMachine = new CombatStateMachine();

        _characterController = new CombatManagerCharacterController(heroes, enemies);

        Subscribe();
    }

    private void Subscribe()
    {
        _stateMachine.OnTurnEnded += OnTurnEndedHandler;
        _characterController.OnCharacterEnterScene += OnCharacterEnterSceneHandler;
        _characterController.OnCharacterLeaveScene += OnCharacterLeaveSceneHandler;

        SubscribeToPreparingState();
        SubscribeToRollingState();
        SubscribeToIdleState();
        SubscribeToAbilityActiveState();

        void SubscribeToPreparingState()
        {
            PreparingState preparingState = _stateMachine.GetState<PreparingState>();

            preparingState.OnStateStarts += OnPreparingStateStartsHandler;
        }

        void SubscribeToRollingState()
        {
            RollingSate rollingState = _stateMachine.GetState<RollingSate>();

            rollingState.OnStateStarts += OnRollingStateStartsHandler;
        }

        void SubscribeToIdleState()
        {
            IdleState idleState = _stateMachine.GetState<IdleState>();

            idleState.OnHeroActivated += OnHeroActivatedHandlder;
        }

        void SubscribeToAbilityActiveState()
        {
            AbilitActiveState abilitActiveState = _stateMachine.GetState<AbilitActiveState>();
            if (abilitActiveState == null) throw new NullReferenceException(nameof(AbilitActiveState));

            //abilitActiveState.OnHeroActivated += OnHeroActivatedHandlder;
            abilitActiveState.OnHeroDeactivated += OnHeroDeactivatedHandlder;
        }
    }

    private void OnDestroy()
    {
        UnsubscribeToPreparingState();
        UnsubscribeToRollingState();
        UnsubscribeToIdleState();
        UnsubscribeToAbilityActiveState();

        _stateMachine.OnTurnEnded -= OnTurnEndedHandler;
        _characterController.OnCharacterEnterScene -= OnCharacterEnterSceneHandler;
        _characterController.OnCharacterLeaveScene -= OnCharacterLeaveSceneHandler;

        void UnsubscribeToPreparingState()
        {
            PreparingState preparingState = _stateMachine.GetState<PreparingState>();

            preparingState.OnStateStarts -= OnPreparingStateStartsHandler;
        }

        void UnsubscribeToRollingState()
        {
            RollingSate rollingState = _stateMachine.GetState<RollingSate>();

            rollingState.OnStateStarts -= OnRollingStateStartsHandler;
        }

        void UnsubscribeToIdleState()
        {
            IdleState idleState = _stateMachine.GetState<IdleState>();

            idleState.OnHeroActivated -= OnHeroActivatedHandlder;
        }

        void UnsubscribeToAbilityActiveState()
        {
            AbilitActiveState abilitActiveState = _stateMachine.GetState<AbilitActiveState>();

            //abilitActiveState.OnHeroActivated -= OnHeroActivated;
            abilitActiveState.OnHeroDeactivated -= OnHeroDeactivated;
        }
    }
    #endregion

    #region properties
    public CombatStateMachine StateMachine => _stateMachine;
    public CombatManagerCharacterController CharacterController => _characterController;
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
    // States
    private void OnPreparingStateStartsHandler()
        => OnPreparingStateStarts?.Invoke();

    private void OnRollingStateStartsHandler()
        => OnRollingStateStarts?.Invoke();

    private void OnTurnEndedHandler()
        => OnTurnEnded?.Invoke();

    private void OnHeroActivatedHandlder(Hero hero)
        => OnHeroActivated?.Invoke(hero);

    private void OnHeroDeactivatedHandlder(Hero hero)
        => OnHeroDeactivated?.Invoke(hero);

    private void OnCharacterEnterSceneHandler(Character character)
        => OnCharacterEnterScene?.Invoke(character);

    private void OnCharacterLeaveSceneHandler(Character character)
        => OnCharacterLeaveScene?.Invoke(character);
    #endregion
}
