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

    [Header("Heroes")]
    [SerializeField] private HeroSO _hero1;
    [SerializeField] private HeroSO _hero2;
    [SerializeField] private HeroSO _hero3;
    [SerializeField] private HeroSO _hero4;
    [SerializeField] private HeroSO _hero5;

    [Header("Enemies")]
    [SerializeField] private List<EnemySO> _enemies;

    

    #region init
    private void Start()
    {
        //_characterManager.Setup(new List<HeroSO> { _hero1, _hero2, _hero3, _hero4, _hero5 }, _enemies);
        _characterManager.Setup(new List<HeroSO> { _hero1 }, _enemies);
        _combatManager.Setup();
        _uiManager.Setup(_characterManager.GetHeroes(), _characterManager.GetEnemies());

        // Event Subscription
        _characterManager.OnCharacterCreated += OnCharacterCreated;
    }

    private void OnDestroy()
    {
        // Event Unsubscription
        _characterManager.OnCharacterCreated -= OnCharacterCreated;
    }
    #endregion

    #region event hanlers
    private void OnCharacterCreated(Character character)
        => _uiManager.AddCharacter(character);
    #endregion
}
