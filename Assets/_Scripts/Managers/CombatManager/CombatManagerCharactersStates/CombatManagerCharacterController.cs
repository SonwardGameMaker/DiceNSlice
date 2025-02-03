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
    public CombatManagerCharacterController(List<Hero> heroes = null, List<Enemy> enemies = null)
    {
        if (heroes == null)
            heroes = new List<Hero>();
        else
            AddHeroes(heroes);

        _deadHeroes = new List<Hero>();

        _currentPoolEnemySize = 0;
        if (enemies == null)
            enemies = new List<Enemy>();
        else
        {
            AddEnemies(enemies);
        }
        _deadEnemies = new List<Enemy>();
    }
    #endregion

    #region external interactions heroes
    public void AddHeroes(List<Hero> heroes)
    {
        if (_presentHeroes == null) _presentHeroes = new List<Hero>();
        if (_presentHeroes.Count + heroes.Count > MaxHeroCount)
            return;

        _presentHeroes.AddRange(heroes);
        _presentHeroes = _presentHeroes.OrderBy(h => (int)h.HeroClass).ToList();
        foreach (Hero hero in _presentHeroes)
        {
            hero.OnCharacterChanged += OnCharacterChangedHandler;
        }
    }

    public void AddHero(Hero hero)
    {
        if (_presentHeroes.Count >= MaxHeroCount) return;
        
        _presentHeroes.Add(hero);
        hero.OnCharacterChanged += OnCharacterChangedHandler;
    }

    public void HeroDies(Hero hero)
    {
        _deadHeroes.Add(hero);
        _presentHeroes.Remove(hero);
        OnCharacterLeaveScene?.Invoke(hero);
        hero.OnCharacterChanged -= OnCharacterChangedHandler;
    }
    #endregion

    #region external interactions enemies
    public void AddEnemies(List<Enemy> enemies)
    {
        if (_presentEnemies == null) _presentEnemies = new List<Enemy>();

        (List<Enemy>, List<Enemy>) result = SplitIntoReinforcements(enemies);
        _presentEnemies.AddRange(result.Item1);
        _reinforcementEnemies.AddRange(result.Item2);
        RefreshEnemiesData();
        foreach (Enemy enemy in result.Item1)
            enemy.OnCharacterChanged += OnCharacterChangedHandler;
    }

    public void AddEnemy(Enemy enemy)
    {
        if (_currentPoolEnemySize + (int)enemy.CharacterSize <= MaxPoolEnemySize)
        {
            _presentEnemies.Add(enemy);
            RefreshEnemiesData();
            enemy.OnCharacterChanged += OnCharacterChangedHandler;
        }
        else
            _reinforcementEnemies.Add(enemy);
    }

    public void MoveEnemyToReinforcements(Enemy enemy, List<Enemy> enemyList = null)
    {
        if (enemyList == null) enemyList = _presentEnemies;
        if (enemyList == _reinforcementEnemies) return;

        _reinforcementEnemies.Add(enemy);
        enemyList.Remove(enemy);
        RefreshEnemiesData();
        enemy.OnCharacterChanged -= OnCharacterChangedHandler;
        OnCharacterLeaveScene?.Invoke(enemy);
    }

    public void EnemyDies(Enemy enemy)
    {
        _deadEnemies.Add(enemy);
        _presentEnemies.Remove(enemy);

        RefreshEnemiesData();
        enemy.OnCharacterChanged -= OnCharacterChangedHandler;
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
    #endregion

    #region event trigers
    private void OnCharacterChangedHandler<T>(T character) where T : Character
    {
        if (character.CurrentHealth <= 0)
        {
            if (character is Hero hero)
                HeroDies(hero);
            else if (character is Enemy enemy)
                EnemyDies(enemy);
        }
    }

    #endregion
}
