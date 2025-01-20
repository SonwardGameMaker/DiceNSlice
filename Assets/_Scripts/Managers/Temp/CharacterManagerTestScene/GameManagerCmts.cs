using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManagerCmts : GameManagerBase
{
    #region fields
    [SerializeField] private CharacterManager _characterManager;
    [SerializeField] private UiManager _uiManager;
    [SerializeField] private InputManager _inputManager;
    [SerializeField] private InputManagerCmts _inputManagerCmts;

    private HashSet<HeroSO> _heroes;
    private HashSet<EnemySO> _enemies;
    #endregion

    #region init
    private void Start()
    {
        _characterManager.Setup();
        _uiManager.Setup();

        // Event Subscription
        CharacterManagerSubscribe();
        CombatManagerSubscription();
        UiManagerSubscription();
        InputManagerSubscription();
    }

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

    protected override void CombatManagerSubscription()
    {
        
    }

    protected override void CombatManagerUnsubscription()
    {
        
    }

    protected override void InputManagerSubscription()
    {
        _inputManager.OnInteractClicked += OnInteractionClickedHandler;
    }

    protected override void InputManagerUnsubscription()
    {
        _inputManager.OnInteractClicked -= OnInteractionClickedHandler;
    }

    private void InputManagerCmtsSubscription()
    {
        // Heroes
        _inputManagerCmts.OnCreateHeroClicked += HandleCreateHero;
        _inputManagerCmts.OnHeroDiesClicked += HandleHeroDies;
        _inputManagerCmts.OnHeroUndiesClicked += HandleHeroUndies;
        _inputManagerCmts.OnResetHeroClicked += HandleResetHero;

        // Enemies
        _inputManagerCmts.OnCreateEnemyClicked += HandleCreateEnemy;
        _inputManagerCmts.OnEnemyDiesClicked += HandleEnemyDies;
        _inputManagerCmts.OnEnemyUndiesClicked += HandleEnemyUndies;
        _inputManagerCmts.OnEnemyResetClicked += HandleResetEnemy;
    }

    private void InputManagerCmtsUnsubscription()
    {
        // Heroes
        _inputManagerCmts.OnCreateHeroClicked -= HandleCreateHero;
        _inputManagerCmts.OnHeroDiesClicked -= HandleHeroDies;
        _inputManagerCmts.OnHeroUndiesClicked -= HandleHeroUndies;
        _inputManagerCmts.OnResetHeroClicked -= HandleResetHero;

        // Enemies
        _inputManagerCmts.OnCreateEnemyClicked -= HandleCreateEnemy;
        _inputManagerCmts.OnEnemyDiesClicked -= HandleEnemyDies;
        _inputManagerCmts.OnEnemyUndiesClicked -= HandleEnemyUndies;
        _inputManagerCmts.OnEnemyResetClicked -= HandleResetEnemy;
    }

    protected override void UiManagerSubscription()
    {
        
    }

    protected override void UimanagerUnsubscription()
    {
        
    }
    #endregion

    #region Character Manager
    private void OnCharacterCreatedHandler(Character character)
        => _uiManager.AddCharacter(character);

    private void OnCharacterRemovedHandler(Character character)
        => _uiManager.RemoveCharacter(character);

    private void OnCharacterChangedHandler(Character character)
    {
        // TODO
    }
    #endregion

    #region UI Manager event handlers

    #endregion

    #region Input Manager handlers
    private void OnInteractionClickedHandler(Vector3 position)
    {
        List<RaycastResult> raycastResults = _uiManager.GetRaycastResults(position);
        bool isOnUi = _uiManager.IsUiElementSelected(raycastResults);
        Character isOnCharacter = _uiManager.IsCharacterSelected(raycastResults);

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
    #endregion

    #region Input Manager Cmts handlers
    // Heroes
    private void HandleCreateHero()
    {
        // TODO: Додайте код для обробки створення героя
    }

    private void HandleHeroDies()
    {
        // TODO: Додайте код для обробки смерті героя
    }

    private void HandleHeroUndies()
    {
        // TODO: Додайте код для обробки відродження героя
    }

    private void HandleResetHero()
    {
        // TODO: Додайте код для обробки скидання героя
    }

    // Enemies
    private void HandleCreateEnemy()
    {
        // TODO: Додайте код для обробки створення ворога
    }

    private void HandleEnemyDies()
    {
        // TODO: Додайте код для обробки смерті ворога
    }

    private void HandleEnemyUndies()
    {
        // TODO: Додайте код для обробки відродження ворога
    }

    private void HandleResetEnemy()
    {
        // TODO: Додайте код для обробки скидання ворога
    }
    #endregion
}
