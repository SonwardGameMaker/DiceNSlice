using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CombatManagerCharacterController
{
    #region fields
    private const int MaxHeroCount = 5;
    private const int MaxPoolEnemySize = 10;

    // Characters
    // Heroes
    private List<Hero> _presentHeroes;
    private List<Hero> _deadHeroes;
    // Enemies
    private List<Enemy> _presentEnemies;
    private List<Enemy> _deadEnemies;
    private List<Enemy> _reinforcementEnemies;

    private int _currentPoolEnemySize;
    #endregion

    #region events
    public event Action<Character> OnCharacterEnterScene;
    public event Action<Character> OnCharacterLeaveScene;
    #endregion

    #region properties
    public List<Hero> PresentHeroes => _presentHeroes;
    public List<Hero> DeadHeroes => _deadHeroes;

    public List<Enemy> PresentEnemies => _presentEnemies;
    public List<Enemy> DeadEnemies => _deadEnemies;
    public List<Enemy> EnemyReinforcements => _reinforcementEnemies;

    public int CurrentEnemyPoolSize => _currentPoolEnemySize;
    #endregion

    #region init
    public CombatManagerCharacterController()
    {
        _presentHeroes = new List<Hero>();
        _deadHeroes = new List<Hero>();

        _presentEnemies = new List<Enemy>();
        _deadEnemies = new List<Enemy>();
        _reinforcementEnemies = new List<Enemy>();

        _currentPoolEnemySize = 0;
    }

    public CombatManagerCharacterController(List<Hero> heroes = null, List<Enemy> enemies = null)
    {
        if (heroes == null)
            heroes = new List<Hero>();
        else
            _presentHeroes = heroes;

        _deadHeroes = new List<Hero>();

        _currentPoolEnemySize = 0;
        if (enemies == null)
            enemies = new List<Enemy>();
        else
        {
            if (CalculateCharSizePool(enemies) <= MaxPoolEnemySize)
            {
                _presentEnemies = enemies;
                _reinforcementEnemies = new List<Enemy>();

                // Tests
                _presentEnemies[0].ChangeShields(2);
            }
            else
            {
                (List<Enemy>, List<Enemy>) result = SplitIntoReinforcements(enemies);
                _presentEnemies = result.Item1;
                _reinforcementEnemies = result.Item2;
            }
            RefreshEnemiesData();
        }
        _deadEnemies = new List<Enemy>();
    }
    #endregion

    #region external interactions heroes
    public void AddHero(Hero hero)
        => _presentHeroes.Add(hero);

    public void HeroDies(Hero hero)
    {
        _deadHeroes.Add(hero);
        _presentHeroes.Remove(hero);
        OnCharacterLeaveScene?.Invoke(hero);
    }
    #endregion

    #region external interactions enemies
    public void AddEnemies(List<Enemy> enemies)
    {
        (List<Enemy>, List<Enemy>) result = SplitIntoReinforcements(enemies);
        _presentEnemies.AddRange(result.Item1);
        _reinforcementEnemies.AddRange(result.Item2);
        RefreshEnemiesData();
    }

    public void MoveEnemyToReinforcements(Enemy enemy, List<Enemy> enemyList = null)
    {
        if (enemyList == null) enemyList = _presentEnemies;
        if (enemyList == _reinforcementEnemies) return;

        _reinforcementEnemies.Add(enemy);
        enemyList.Remove(enemy);
        RefreshEnemiesData();
        OnCharacterLeaveScene?.Invoke(enemy);
    }

    public void EnemyDies(Enemy enemy)
    {
        _deadEnemies.Add(enemy);
        _presentEnemies.Remove(enemy);

        RefreshEnemiesData();
        TryGetEnemyFromReinforcements();
        OnCharacterLeaveScene?.Invoke(enemy);
    }

    #endregion

    #region internal operations
    /// <summary>
    /// First list is for active enemies, second - for reinforcement
    /// </summary>
    /// <param name="enemies"></param>
    /// <returns></returns>
    private (List<Enemy>, List<Enemy>) SplitIntoReinforcements(List<Enemy> enemies)
    {
        List<Enemy> activeEnemies = new List<Enemy>();
        List<Enemy> reinforcementsEnemy = new List<Enemy>();

        int activeSizePull = _currentPoolEnemySize;
        int count = 0;
        while (activeSizePull <= MaxPoolEnemySize)
        {
            AddToActive();
            count++;
        }

        for (int i = count; i < enemies.Count; i++)
            reinforcementsEnemy.Add(enemies[i]);

        while (activeSizePull <= MaxPoolEnemySize && count < enemies.Count)
        {
            if (activeSizePull + (int)enemies[count].CharacterSize <= MaxPoolEnemySize)
            {
                AddToActive();
            }
            count++;
        }

        return new(activeEnemies, reinforcementsEnemy);

        // Utils
        void AddToActive()
        {
            activeEnemies.Add(enemies[count]);
            activeSizePull += (int)enemies[count].CharacterSize;
        }
    }

    private bool TryGetEnemyFromReinforcements()
    {
        if (_reinforcementEnemies == null || _reinforcementEnemies.Count == 0) return false;

        foreach (Enemy enemy in _reinforcementEnemies)
            if (_currentPoolEnemySize + (int)enemy.CharacterSize <= MaxPoolEnemySize)
            {
                _presentEnemies.Add(enemy);
                _reinforcementEnemies.Remove(enemy);
                RefreshEnemiesData();
                OnCharacterEnterScene?.Invoke(enemy);
            }

        return true;
    }

    private int CalculateCharSizePool<T>(List<T> characters) where T : Character
    => characters.Sum(c => (int)c.CharacterSize);

    private void RefreshEnemiesData()
    {
        if (CalculateCharSizePool(_presentEnemies) > MaxPoolEnemySize)
        {
            // make full reset from GameObjects
        }

        _currentPoolEnemySize = CalculateCharSizePool(_presentEnemies);
    }

    private void CharacterDies<T>(T charatcer, List<T> aliveCharatcers, List<T> deadCharacters) where T : Character
    {
        deadCharacters.Add(charatcer);
        aliveCharatcers.Remove(charatcer);
    }

    private void CharacterUndies<T>(T charatcer, List<T> aliveCharatcers, List<T> deadCharacters) where T : Character
    {
        aliveCharatcers.Add(charatcer);
        deadCharacters.Remove(charatcer);
    }

    private void ClearUpList<T>(List<T> list) where T : Character
        => list.RemoveAll(c => c.gameObject == null);

    private void CleanUpAllCharacterLists()
    {
        _presentHeroes.RemoveAll(h => h.gameObject == null);
        _deadEnemies.RemoveAll(h => h.gameObject == null);

        _presentEnemies.RemoveAll(e => e.gameObject == null);
        _deadEnemies.RemoveAll(e => e.gameObject == null);
        _reinforcementEnemies.RemoveAll(e => e.gameObject == null);
    }
    #endregion
}
