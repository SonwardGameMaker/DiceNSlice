using System;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour, ICombatManager
{
    #region fields
    private ICharacterManager _characterManager;

    private CombatStateMachine _stateMachine;
    private ICombatCharacterLists _combatLists;
    #endregion

    #region events
    // States
    public event Action OnPreparingStateStarts;
    public event Action OnRollingStateStarts;

    // Characters
    public event Action<Hero> OnHeroActivated;
    public event Action<Hero> OnHeroDeactivated;

    // Combat flow
    public event Action<bool> OnCombatEnded;
    #endregion

    #region init
    public void Setup(ICharacterManager charactcerManager)
    {
        _characterManager = charactcerManager;
        _combatLists = new CombatCharacterLists(charactcerManager.Heroes, charactcerManager.Enemies);
        _stateMachine = new CombatStateMachine(_combatLists);

        SubscribeToInnerStates();
        SubscribeToCharacterManager();
    }

    private void SubscribeToCharacterManager()
    {
        _characterManager.OnCharacterCreated += OnCharacterCreatedHandler;
        _characterManager.OnCharacterDeleted += OnCharacterDeletelHandler;
        _characterManager.OnCharacterChanged += OnCharacterChangedHandler;
    }

    private void SubscribeToInnerStates()
    {
        _stateMachine.OnTurnEnded += OnTurnEndedHandler;

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

            abilitActiveState.OnHeroDeactivated += OnHeroDeactivatedHandlder;
        }
    }

    private void OnDestroy()
    {
        UnsubscribeToCharacterManager();

        UnsubscribeToPreparingState();
        UnsubscribeToRollingState();
        UnsubscribeToIdleState();
        UnsubscribeToAbilityActiveState();

        _stateMachine.OnTurnEnded -= OnTurnEndedHandler;

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

            abilitActiveState.OnHeroDeactivated -= OnHeroDeactivated;
        }


        void UnsubscribeToCharacterManager()
        {
            _characterManager.OnCharacterCreated -= OnCharacterCreatedHandler;
            _characterManager.OnCharacterDeleted -= OnCharacterDeletelHandler;
            _characterManager.OnCharacterChanged -= OnCharacterChangedHandler;
        }
    }
    #endregion

    #region properties
    public CombatStateMachine StateMachine => _stateMachine;
    public ICombatCharacterLists CombatLists => _combatLists;
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
    // Character Manager
    private void OnCharacterCreatedHandler(Character character)
    {
        if (character is Hero hero)
            _combatLists.AddHero(hero);
        else if (character is Enemy enemy)
            _combatLists.AddEnemy(enemy);
        else
            return;
    }

    private void OnCharacterChangedHandler(Character character)
        => _combatLists.OnCharacterChangedHandler(character);

    private void OnCharacterDeletelHandler(Character character)
    {
        if (character is Hero hero)
            _combatLists.RemoveHero(hero);
        else if (character is Enemy enemy)
            _combatLists.RemoveEnemy(enemy);
        else
            return;
    }

    // States
    private void OnPreparingStateStartsHandler()
        => OnPreparingStateStarts?.Invoke();

    private void OnRollingStateStartsHandler()
        => OnRollingStateStarts?.Invoke();

    private void OnTurnEndedHandler()
    {

    }

    private void OnHeroActivatedHandlder(Hero hero)
        => OnHeroActivated?.Invoke(hero);

    private void OnHeroDeactivatedHandlder(Hero hero)
        => OnHeroDeactivated?.Invoke(hero);
    #endregion
}
