using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TempGameManager : MonoBehaviour
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
        _uiManager.Setup(_characterManager.GetHeroes(), _characterManager.GetEnemies());

        // Event Subscription
        CharacterManagerSubscribe();
        CombatManagerSubscription();
        UiManagerSubscription();
        InputManagerSubscription();
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
    private void CharacterManagerSubscribe()
    {
        _characterManager.OnCharacterCreated += OnCharacterCreatedHandler;
        _characterManager.OnCharacterDeleted += OnCharacterRemovedHandler;
    }
    private void CharacterManagerUnsubscribe()
    {
        _characterManager.OnCharacterCreated -= OnCharacterCreatedHandler;
        _characterManager.OnCharacterDeleted -= OnCharacterRemovedHandler;
    }

    // Combat Manager
    private void CombatManagerSubscription()
    {
        _combatManager.OnHeroActivated += OnHeroActivatedHandler;
        _combatManager.OnHeroDeactivated += OnHeroDeactivatedHandler;
    }
    private void CombatManagerUnsubscription()
    {
        _combatManager.OnHeroActivated -= OnHeroActivatedHandler;
        _combatManager.OnHeroDeactivated -= OnHeroDeactivatedHandler;
    }

    // UI Manager
    private void UiManagerSubscription()
    {

    }
    private void UimanagerUnsubscription()
    {

    }

    // Input Manager
    private void InputManagerSubscription()
    {
        _inputManager.OnInteractClicked += OnInteractionClickedHandler;
        _inputManager.OnInfoClicked += OnInfoClickedHandler;

        // Buttons
        _inputManager.OnRerollClicked += OnRerollClickedHandler;
        _inputManager.OnCancelRerollClicked += OnCancelRerollClickedHandler;
        _inputManager.OnDoneRerollingClicked += OnDoneRerollingClickedHandler;
        _inputManager.OnNextTurnClicked += OnNextTurnClickedHandler;
    }
    private void InputManagerUnsubscription()
    {
        _inputManager.OnInteractClicked -= OnInteractionClickedHandler;
        _inputManager.OnInfoClicked -= OnInfoClickedHandler;

        // Buttons
        _inputManager.OnRerollClicked -= OnRerollClickedHandler;
        _inputManager.OnCancelRerollClicked -= OnCancelRerollClickedHandler;
        _inputManager.OnDoneRerollingClicked -= OnDoneRerollingClickedHandler;
        _inputManager.OnNextTurnClicked -= OnNextTurnClickedHandler;
    }
    #endregion

    #region Character Manager event handlers
    private void OnCharacterCreatedHandler(Character character)
        => _uiManager.AddCharacter(character);

    private void OnCharacterRemovedHandler(Character character)
        => _uiManager.RemoveCharacter(character);
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
    private void OnCharacterClickedHandler(Character character)
    {
        // Temp debug realization, TODO
        _uiManager.MoveCharacterForvard(character);
    }
    #endregion

    #region Input Manager handlers
    private void OnInteractionClickedHandler(Vector3 position)
    {
        bool isOnUi = _uiManager.IsUiElementSelected(position);
        Character isOnCharacter = _uiManager.IsCharacterSelected(position);

        if (isOnCharacter) // Check if this not null check
        {
            _combatManager.SelectCharacter(isOnCharacter);
            return;
        }

        _combatManager.Cancel();

        if (isOnUi)
        {
            foreach (var hero in _characterManager.GetHeroes())
                _uiManager.MoveCharacterBack(hero);
        }
        else
        {
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
    }

    #endregion
}
