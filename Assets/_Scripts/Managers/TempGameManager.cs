using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

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
    }
    protected override void CharacterManagerUnsubscribe()
    {
        _characterManager.OnCharacterCreated -= OnCharacterCreatedHandler;
        _characterManager.OnCharacterDeleted -= OnCharacterRemovedHandler;
        _characterManager.OnCharacterChanged -= OnCharacterChangedHandler;
    }

    // Combat Manager
    protected override void CombatManagerSubscription()
    {
        if (_characterManager == null) throw new NullReferenceException(nameof(CombatManager));

        _combatManager.OnHeroActivated += OnHeroActivatedHandler;
        _combatManager.OnHeroDeactivated += OnHeroDeactivatedHandler;
    }
    protected override void CombatManagerUnsubscription()
    {
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
    {
        // TODO
    }
    #endregion

    #region Combat Manager event handlers
    private void OnHeroActivatedHandler(Hero hero)
    {
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
