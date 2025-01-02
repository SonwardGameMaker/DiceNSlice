using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    #region fields
    [Header("Prefabs")]
    [SerializeField] private GameObject _heroFramePrefab;
    [SerializeField] private GameObject _enemyFramePrefab;

    [Header("UI Components")]
    [SerializeField] private VerticalLayoutGroup _heroes;
    [SerializeField] private VerticalLayoutGroup _enemies;

    private bool _isSet = false;
    #endregion

    #region events
    public event Action<Character> OnCharacterClicked;
    #endregion

    #region init
    public void Setup(List<Hero> heroes, List<Enemy> enemies)
    {
        if (_isSet) return;

        SetHeroes(heroes);
        SetEnemies(enemies);
    }
    #endregion

    private void Update()
    {
        // TODO trigger OnCharacterClicked
    }

    #region external interactions
    public void SetHeroes(List<Hero> heroes)
    {
        // TODO
    }

    public void SetEnemies(List<Enemy> enemies)
    {
        // TODO
    }

    public void AddHero(Hero hero)
    {
        // TODO
    }

    public void RemoveHero(Hero hero)
    {
        // TODO
    }

    public void AddEnemy(Enemy enemy)
    {
        GameObject go = Instantiate(_enemyFramePrefab, Vector3.zero, Quaternion.identity);
        go.transform.SetParent(_enemies.transform); // test if this work properly
    }

    public void RemoveEnemy(Enemy enemy)
    {
        // TODO
    }
    #endregion
}
