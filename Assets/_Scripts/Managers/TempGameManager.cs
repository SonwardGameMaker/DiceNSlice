using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TempGameManager : MonoBehaviour
{
    [SerializeField] private CombatManager _combatManager;
    [SerializeField] private CharacterManager _characterManager;
    [SerializeField] private UiManager _uiManager;

    [Header("Init")]
    [SerializeField] private TempGameManagerInitSO _initSO;


    #region init
    private void Start()
    {
        //_characterManager.Setup(new List<HeroSO> { _hero1, _hero2, _hero3, _hero4, _hero5 }, _enemies);
        _characterManager.Setup(_initSO.Heroes, _initSO.Enemies);
        _combatManager.Setup();
        _uiManager.Setup(_characterManager.GetHeroes(), _characterManager.GetEnemies());

        // Event Subscription
        _characterManager.OnCharacterCreated += OnCharacterCreatedHandler;
        _characterManager.OnCharacterDeleted += OnCharacterRemovedHandler;
    }

    private void OnDestroy()
    {
        // Event Unsubscription
        _characterManager.OnCharacterCreated -= OnCharacterCreatedHandler;
        _characterManager.OnCharacterDeleted -= OnCharacterRemovedHandler;
    }
    #endregion

    #region event handlers
    private void OnCharacterCreatedHandler(Character character)
        => _uiManager.AddCharacter(character);

    private void OnCharacterRemovedHandler(Character character)
        => _uiManager.RemoveCharacter(character);
    #endregion
}
