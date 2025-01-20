using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    private const int MaxPoolEnemySize = 10;

    #region fields
    [Header("Containers")]
    [SerializeField] private Transform _heroContainer;
    [SerializeField] private Transform _enemyContainer;

    // heroes
    private List<Hero> _heroes;
    private List<Hero> _deadHeroes;
    // enemies
    private List<Enemy> _enemies;
    private List<Enemy> _deadEnemies;
    private List<Enemy> _reinforcementsEnemies;

    private bool _enemiesInTwoLines;
    private int _currentPoolEnemySize;
    #endregion

    #region events
    public event Action<Character> OnCharacterCreated;
    public event Action<Character> OnCharacterDeleted;
    public event Action<Character> OnCharacterChanged;
    #endregion

    #region init
    public void Setup()
    {
        _heroes = new List<Hero>();
        _deadHeroes = new List<Hero>();

        _enemies = new List<Enemy>();
        _deadEnemies = new List<Enemy>();
        _reinforcementsEnemies = new List<Enemy>();

        _enemiesInTwoLines = false;
        _currentPoolEnemySize = 0;
    }

    public void Setup(List<HeroSO> heroes, List<EnemySO> enemies)
        => Setup(CreateHeroes(heroes), CreateEnemies(enemies));

    public void Setup(List<Hero> heroes, List<Enemy> enemies)
    {
        _heroes = heroes;

        if (CalculateCharSizePool(enemies) <= MaxPoolEnemySize)
        {
            _enemies = enemies;
            _reinforcementsEnemies = new List<Enemy>();
        }
        else
        {
            (List<Enemy>, List<Enemy>) result = SplitIntoReinforcements(enemies);
            _enemies = result.Item1;
            _reinforcementsEnemies = result.Item2;
        }

    }
    #endregion

    #region properties
    public bool EnemiesInTwoLines => _enemiesInTwoLines;
    #endregion

    #region exteral interactions
    public List<Hero> GetHeroes()
    {
        List<Hero> result = new List<Hero>();
        for (int i = 0; i < _heroContainer.childCount; i++)
            result.Add(_heroContainer.GetChild(i).GetComponent<Hero>());
        return result;
    }

    public List<Hero> CreateHeroes(List<HeroSO> heroes)
        => heroes.Select(h => CreateCharacter(h) as Hero).ToList();

    public List<Enemy> GetEnemies()
    {
        List<Enemy> result = new List<Enemy>();
        for (int i = 0; i < _enemyContainer.childCount; i++)
            result.Add(_enemyContainer.GetChild(i).GetComponent<Enemy>());
        return result;
    }

    public List<Enemy> CreateEnemies(List<EnemySO> enemies)
        => enemies.Select(h => CreateCharacter(h) as Enemy).ToList();

    public void AddEnemy(EnemySO so)
    {
        Enemy enemy = CreateCharacter(so) as Enemy;

        if ((int)enemy.CharacterSize + CalculateCharSizePool(_enemies) <= MaxPoolEnemySize)
        { 
            _enemies.Add(enemy); 
            CheckEnemiesLines();
        }
        else
            _reinforcementsEnemies.Add(enemy);
    }

    public void CheckEnemiesLines()
    {
        if (_enemies == null || _enemies.Count == 0) return;

        bool onBackline = _enemies[0].OnBackline;
        foreach (Enemy enemy in _enemies)
            if (enemy.OnBackline != onBackline)
                _enemiesInTwoLines = true;
            else 
                _enemiesInTwoLines = false;
    }
    
    public void HeroDies(Hero hero)
    {

    }

    public void EnemyDies(Enemy enemy)
    {
        _deadEnemies.Add(enemy);
        _enemies.Remove(enemy);
        TryGetEnemyFromReinforcements();
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

        int activeSizePull = 0;
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

    private int CalculateCharSizePool<T>(List<T> characters) where T : Character
        => characters.Sum(c => (int)c.CharacterSize);

    private Character CreateCharacter(CharacterSO so)
    {
        string nameSuffix;
        Type type;
        Transform container;

        if (so is HeroSO)
        {
            nameSuffix = "_Hero";
            type = typeof(Hero);
            container = _heroContainer;
        }
        else
        {
            nameSuffix = "_Enemy";
            type = typeof(Enemy);
            container = _enemyContainer;
        }

        GameObject character = new GameObject(so.Name + nameSuffix);
        character.transform.SetParent(container.transform);
        Character result = character.AddComponent(type) as Character;
        character.AddComponent<Dice>();
        result.Setup(so);

        OnCharacterCreated?.Invoke(result);
        result.OnCharacterChanged += OnCharacterChangedHandler;

        return result;
    }

    private bool RemoveCharacter(Character character)
    {
        Transform container;

        if (character is Hero)
        {
            container = _heroContainer;
        }
        else
        {
            container = _enemyContainer;
        }

        if (container == null || character == null)
            return false;

        foreach (Transform child in container)
        {
            Character childCharacter = child.GetComponent<Character>();
            if (childCharacter != null && childCharacter == character)
            {
                character.OnCharacterChanged -= OnCharacterChangedHandler;
                OnCharacterDeleted?.Invoke(character);
                Destroy(child.gameObject);
                return true;
            }
        }

        if (character is Enemy)
            CheckEnemiesLines();

        return false;
    }

    private bool TryGetEnemyFromReinforcements()
    {
        int currentPoolSize = CalculateCharSizePool(_enemies);

        if (_reinforcementsEnemies == null || _reinforcementsEnemies.Count == 0) return false;

        foreach (Enemy enemy in _reinforcementsEnemies)
            if (currentPoolSize + (int)enemy.CharacterSize <= MaxPoolEnemySize)
            {
                _enemies.Add(enemy);
                _reinforcementsEnemies.Remove(enemy);
                currentPoolSize = CalculateCharSizePool(_enemies);
            }

        return true;
    }
    #endregion

    #region event hanlers
    private void OnCharacterChangedHandler(Character character)
    {
        CheckEnemiesLines();
        OnCharacterChanged?.Invoke(character);
    }
    #endregion
}
