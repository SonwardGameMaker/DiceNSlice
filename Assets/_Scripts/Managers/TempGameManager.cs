using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.EventSystems.EventTrigger;

public class TempGameManager : GameManagerBase
{
    #region fields
    [SerializeField] private CharacterManager _characterManager;
    [SerializeField] private CombatManager _combatManager;
    [SerializeField] private UiManager _uiManager;
    [SerializeField] private InputManager _inputManager;

    [Header("Init")]
    [SerializeField] private TempGameManagerInitSO _initSO;
    #endregion

    #region init
    private void Start()
    {
        _characterManager.Setup(_initSO.Heroes, _initSO.Enemies);
        _combatManager.Setup();
        _uiManager.Setup(_characterManager.Heroes, _characterManager.Enemies);

        // Event Subscription
        CharacterManagerSubscribe();
        CombatManagerSubscription();
        UiManagerSubscription();
        InputManagerSubscription();

        _combatManager.StartCombat();
    }

    private void OnDestroy()
    {
        // Event Unsubscription
        CharacterManagerUnsubscribe();
        CombatManagerUnsubscription();
        UimanagerUnsubscription();
        InputManagerUnsubscription();
    }

    // Character Manager
    protected override void CharacterManagerSubscribe()
    {
        _characterManager.OnCharacterCreated += OnCharacterCreatedHandler;
        _characterManager.OnCharacterDeleted += OnCharacterRemovedHandler;
        _characterManager.OnCharacterChanged += OnCharacterChangedHandler;

        _characterManager.OnCharacterEnterScene += OnCharacterEnterSceneHandler;
        _characterManager.OnCharacterLeaveScene += OnCharacterLeaveScene;
    }
    protected override void CharacterManagerUnsubscribe()
    {
        _characterManager.OnCharacterCreated -= OnCharacterCreatedHandler;
        _characterManager.OnCharacterDeleted -= OnCharacterRemovedHandler;
        _characterManager.OnCharacterChanged -= OnCharacterChangedHandler;

        _characterManager.OnCharacterEnterScene += OnCharacterEnterSceneHandler;
        _characterManager.OnCharacterLeaveScene += OnCharacterLeaveScene;
    }

    // Combat Manager
    protected override void CombatManagerSubscription()
    {
        if (_characterManager == null) throw new NullReferenceException(nameof(CombatManager));

        _combatManager.OnPreparingStateStarts += OnPreparingStateStartsHandler;
        _combatManager.OnPreparingStateStarts += OnRollingStateStartsHandler;

        _combatManager.OnHeroActivated += OnHeroActivatedHandler;
        _combatManager.OnHeroDeactivated += OnHeroDeactivatedHandler;
    }
    protected override void CombatManagerUnsubscription()
    {
        _combatManager.OnPreparingStateStarts -= OnPreparingStateStartsHandler;
        _combatManager.OnPreparingStateStarts -= OnRollingStateStartsHandler;

        _combatManager.OnHeroActivated -= OnHeroActivatedHandler;
        _combatManager.OnHeroDeactivated -= OnHeroDeactivatedHandler;
    }

    // UI Manager
    protected override void UiManagerSubscription()
    {

    }
    protected override void UimanagerUnsubscription()
    {

    }

    // Input Manager
    protected override void InputManagerSubscription()
    {
        _inputManager.OnInteractClicked += OnInteractionClickedHandler;
        _inputManager.OnInfoClicked += OnInfoClickedHandler;

        // Buttons
        _inputManager.OnRerollClicked += OnRerollClickedHandler;
        _inputManager.OnCancelRerollClicked += OnCancelRerollClickedHandler;
        _inputManager.OnDoneRerollingClicked += OnDoneRerollingClickedHandler;
        _inputManager.OnNextButtonClicked += OnNextTurnClickedHandler;
    }
    protected override void InputManagerUnsubscription()
    {
        _inputManager.OnInteractClicked -= OnInteractionClickedHandler;
        _inputManager.OnInfoClicked -= OnInfoClickedHandler;

        // Buttons
        _inputManager.OnRerollClicked -= OnRerollClickedHandler;
        _inputManager.OnCancelRerollClicked -= OnCancelRerollClickedHandler;
        _inputManager.OnDoneRerollingClicked -= OnDoneRerollingClickedHandler;
        _inputManager.OnNextButtonClicked -= OnNextTurnClickedHandler;
    }
    #endregion

    #region Character Manager event handlers
    private void OnCharacterCreatedHandler(Character character)
        => _uiManager.AddCharacter(character);

    private void OnCharacterRemovedHandler(Character character)
        => _uiManager.RemoveCharacter(character);

    private void OnCharacterChangedHandler(Character character)
        => _uiManager.UpdateCharacter(character);

    private void OnCharacterEnterSceneHandler(Character character)
        => _uiManager.EnableCharacter(character);

    private void OnCharacterLeaveScene(Character character)
        => _uiManager.DisableCharacter(character);
    #endregion

    #region Combat Manager event handlers
    private void OnPreparingStateStartsHandler()
    {
        // Temp realization, TODO with DiceManager
        foreach (Enemy enemy in _characterManager.Enemies)
        {
            enemy.Dice.RollTheDice();
            _uiManager.GetCharacterFrame(enemy).SetRolledDice();
        }
        _combatManager.Next();
    }

    private void OnRollingStateStartsHandler()
    {
        // Temp realization, TODO with DiceManager
        foreach (Hero hero in _characterManager.Heroes)
        {       
            hero.Dice.RollTheDice();
            _uiManager.GetCharacterFrame(hero).SetRolledDice();
        }
        _combatManager.Next();
    }

    private void OnHeroActivatedHandler(Hero hero)
    {
        if (_combatManager.StateMachine.CurrentState is AbilitActiveState abilityActiveState)
        {
            Dice dice = hero.GetComponent<Dice>();
            abilityActiveState.SetActiveHero(hero, dice, dice.LockedSide.GameAction.GetValidTargets
                (_characterManager.Heroes.Select(h => h as Character).ToList(), _characterManager.Enemies.Select(e => e as Character).ToList()));
        }

        _uiManager.MoveCharacterForvard(hero);
    }

    private void OnHeroDeactivatedHandler(Hero hero)
    {
        _uiManager.MoveCharacterBack(hero);
    }
    #endregion

    #region UI Manager event handlers

    #endregion

    #region Input Manager handlers
    private void OnInteractionClickedHandler(Vector3 position)
    {
        //Debug.Log($"{nameof(OnInfoClickedHandler)} started");

        List<RaycastResult> raycastResults = _uiManager.GetRaycastResults(position);
        bool isOnUi = _uiManager.IsUiElementSelected(raycastResults);
        Character isOnCharacter = _uiManager.IsCharacterSelected(raycastResults);

        if (isOnCharacter != null)
        {
            _combatManager.SelectCharacter(isOnCharacter);
            return;
        }

        _combatManager.Cancel();

        if (isOnUi)
        {
            // TODO
        }
        else
        {
            Vector3 worldPosition = UiManager.ToWorldPosition(position);
            // TODO
        }
    }

    private void OnInfoClickedHandler(Vector3 position)
    {
        // TODO
        //Debug.Log($"Info Clicked on {position}");
    }

    private void OnRerollClickedHandler()
    {
        // TODO: Implement reroll clicked behavior
        Debug.Log("Reroll button clicked.");
    }

    private void OnCancelRerollClickedHandler()
    {
        // TODO: Implement cancel reroll clicked behavior
        Debug.Log("Cancel reroll button clicked.");
    }

    private void OnDoneRerollingClickedHandler()
    {
        // TODO: Implement done rerolling clicked behavior
        Debug.Log("Done rerolling button clicked.");
    }

    private void OnNextTurnClickedHandler()
    {
        // TODO: Implement next turn clicked behavior
        Debug.Log("Next turn button clicked.");

        _combatManager.Next();
    }

    #endregion
}
