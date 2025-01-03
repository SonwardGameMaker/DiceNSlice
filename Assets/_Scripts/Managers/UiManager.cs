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
        foreach (var hero in heroes)
            AddHero(hero);
    }

    public void SetEnemies(List<Enemy> enemies)
    {
        foreach(var enemy in enemies)
            AddEnemy(enemy);
    }

    public void AddHero(Hero hero)
    {
        GameObject go = Instantiate(_heroFramePrefab, Vector3.zero, Quaternion.identity);
        go.transform.SetParent(_heroes.transform); // test if this work properly
        go.GetComponent<CharacterFrame>().Setup(hero);
    }

    public bool RemoveHero(Hero hero)
    {
        for(int i = 0; i < _heroes.transform.childCount; i++)
        {
            var tempHero = _heroes.transform.GetChild(i);
            if (tempHero.GetComponent<CharacterFrame>().Character == hero)
            {
                Destroy(tempHero);
            }
        }

        return false;
    }

    public void AddEnemy(Enemy enemy)
    {
        GameObject go = Instantiate(_enemyFramePrefab, Vector3.zero, Quaternion.identity);
        go.transform.SetParent(_enemies.transform); // test if this work properly
        go.GetComponent<CharacterFrame>().Setup(enemy);
    }

    public void RemoveEnemy(Enemy enemy)
    {
        // TODO
    }
    #endregion
}
