using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.TextCore.Text;
using static UnityEngine.EventSystems.EventTrigger;

public class CharacterManager : MonoBehaviour
{
    #region fields
    private const int MaxHeroCount = 5;
    private const int MaxPoolEnemySize = 10;

    [Header("Containers")]
    [SerializeField] private Transform _heroContainer;
    [SerializeField] private Transform _enemyContainer;

    // heroes
    private List<Hero> _heroes;
    private List<Hero> _deadHeroes;
    // enemies
    private List<Enemy> _enemies;
    private List<Enemy> _deadEnemies;
    private List<Enemy> _reinforcementEnemies;

    private bool _enemiesInTwoLines;
    private int _currentPoolEnemySize;
    #endregion

    #region events
    public event Action<Character> OnCharacterCreated;
    public event Action<Character> OnCharacterDeleted;
    public event Action<Character> OnCharacterChanged;
    public event Action<Character> OnCharacterEnterScene;
    public event Action<Character> OnCharacterLeaveScene;
    #endregion

    #region init
    // Setup for testing classes
    public void TestSetup(Transform heroContainer, Transform enemyContainer)
    {
        _heroContainer = heroContainer;
        _enemyContainer = enemyContainer;

        Setup();
    }

    public void TestSetup(Transform heroContainer, Transform enemyContainer, List<HeroSO> heroes, List<EnemySO> enemies)
    {
        _heroContainer = heroContainer;
        _enemyContainer = enemyContainer;

        Setup(heroes, enemies);
    }

    public void TestSetup(Transform heroContainer, Transform enemyContainer, List<Hero> heroes, List<Enemy> enemies)
    {
        _heroContainer = heroContainer;
        _enemyContainer = enemyContainer;

        Setup(heroes, enemies);
    }

    // Standart setup
    public void Setup()
    {
        _heroes = new List<Hero>();
        _deadHeroes = new List<Hero>();

        _enemies = new List<Enemy>();
        _deadEnemies = new List<Enemy>();
        _reinforcementEnemies = new List<Enemy>();

        _enemiesInTwoLines = false;
        _currentPoolEnemySize = 0;
    }

    public void Setup(List<HeroSO> heroes, List<EnemySO> enemies)
        => Setup(CreateHeroes(heroes), CreateEnemies(enemies));

    private void Setup(List<Hero> heroes, List<Enemy> enemies)
    {
        if (heroes == null)
            heroes = new List<Hero>();
        else
            _heroes = heroes;

        _deadHeroes = new List<Hero>();

        _currentPoolEnemySize = 0;
        if (enemies == null)
            enemies = new List<Enemy>();
        else
        {
            if (CalculateCharSizePool(enemies) <= MaxPoolEnemySize)
            {
                _enemies = enemies;
                _reinforcementEnemies = new List<Enemy>();

                // Tests
                _enemies[0].ChangeShields(2);
            }
            else
            {
                (List<Enemy>, List<Enemy>) result = SplitIntoReinforcements(enemies);
                _enemies = result.Item1;
                _reinforcementEnemies = result.Item2;
            }
            RefreshEnemiesData();
        }
        _deadEnemies = new List<Enemy>();
    }
    #endregion

    #region properties
    public List<Hero> Heroes => _heroes;
    public List<Hero> DeadHeroes => _deadHeroes;

    public List<Enemy> Enemies => _enemies;
    public List<Enemy> DeadEnemies => _deadEnemies;
    public List<Enemy> EnemyReinforcements => _reinforcementEnemies;

    public int CurrentEnemyPoolSize => _currentPoolEnemySize;
    public bool EnemiesInTwoLines => _enemiesInTwoLines;
    #endregion

    #region exteral interactions heroes
    public void AddHeroes(List<HeroSO> sOs)
    {
        if (_heroes.Count + sOs.Count > MaxHeroCount)
            return;

        List<Hero> heroes = CreateHeroes(sOs);

        _heroes.AddRange(heroes);
        _heroes = _heroes.OrderBy(h => (int)h.HeroClass).ToList();
    }

    public void AddHero(HeroSO so)
    {
        if (_heroes.Count >= MaxHeroCount) 
            return;

        _heroes.Add(CreateHero(so));
        _heroes = _heroes.OrderBy(h => (int)h.HeroClass).ToList();
    }

    public void DeleteHero(Hero hero)
    {
        _heroes.Remove(hero);
        _deadHeroes.Remove(hero);
        DeleteCharacter(hero, _heroContainer);
    }

    public void HeroDies(Hero hero)
    {
        _deadHeroes.Add(hero);
        _heroes.Remove(hero);
        OnCharacterLeaveScene?.Invoke(hero);
    }

    public void ResetHeroesShields()
        => ResetShields(_heroes);
    #endregion

    #region exteral interactions enemies
    public void AddEnemies(List<EnemySO> enemies)
    {
        (List<Enemy>, List<Enemy>) result = SplitIntoReinforcements(enemies.Select(h => CreateEnemy(h)).ToList());
        _enemies.AddRange(result.Item1);
        _reinforcementEnemies.AddRange(result.Item2);
        RefreshEnemiesData();
    }

    public void AddEnemy(EnemySO so)
    {
        Enemy enemy = (Enemy)CreateCharacter<Enemy>(so, _enemyContainer, "_Enemy");

        if ((int)enemy.CharacterSize + _currentPoolEnemySize <= MaxPoolEnemySize)
        {
            _enemies.Add(enemy);
            RefreshEnemiesData();
        }
        else
            _reinforcementEnemies.Add(enemy);

    }

    public void DeleteEnemy(Enemy enemy)
    {
        _enemies.Remove(enemy);
        _deadEnemies.Remove(enemy);
        _reinforcementEnemies.Remove(enemy);

        DeleteCharacter(enemy, _enemyContainer);
        RefreshEnemiesData();
        TryGetEnemyFromReinforcements();
    }

    public void EnemyDies(Enemy enemy)
    {
        _deadEnemies.Add(enemy);
        _enemies.Remove(enemy);

        RefreshEnemiesData();
        TryGetEnemyFromReinforcements();
        OnCharacterLeaveScene?.Invoke(enemy);
    }

    public void MoveEnemyToReinforcements(Enemy enemy, List<Enemy> enemyList = null)
    {
        if (enemyList == null) enemyList = _enemies;
        if (enemyList == _reinforcementEnemies) return;

        _reinforcementEnemies.Add(enemy);
        enemyList.Remove(enemy);
        RefreshEnemiesData();
        OnCharacterLeaveScene?.Invoke(enemy);
    }

    public void ResetEnemiesShields()
        => ResetShields(_enemies);
    #endregion

    #region internal operations
    private Character CreateCharacter<T>(CharacterSO so, Transform container, string nameSuffix) where T : Character
    {
        GameObject character = new GameObject(so.Name + nameSuffix);
        character.transform.SetParent(container.transform);

        T result = character.AddComponent<T>();
        Dice dice = character.AddComponent<Dice>();
        result.Setup(so);
        dice.Setup(result, so.GetDiceSides());

        OnCharacterCreated?.Invoke(result);
        result.OnCharacterChanged += OnCharacterChangedHandler;

        return result;
    }

    private bool DeleteCharacters(List<Character> characters, Transform container)
    {
        if (container == null || characters == null || characters.Count == 0)
            return false;

        bool result = false;

        foreach (Character character in characters)
            if (DeleteCharacter(character, container))
                result = true;

        return result;
    }

    private bool DeleteCharacter(Character character, Transform container)
    {
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

        return false;
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
        _heroes.RemoveAll(h => h.gameObject == null);
        _deadEnemies.RemoveAll(h => h.gameObject == null);

        _enemies.RemoveAll(e=> e.gameObject == null);
        _deadEnemies.RemoveAll(e => e.gameObject == null);
        _reinforcementEnemies.RemoveAll(e => e.gameObject == null);
    }

    private int CalculateCharSizePool<T>(List<T> characters) where T : Character
        => characters.Sum(c => (int)c.CharacterSize);

    private void ResetShields<T>(List<T> characters) where T : Character
    {
        foreach (T character in characters)
            character.ResetShields();
    }
    #endregion

    #region internal operations heroes
    private List<Hero> CreateHeroes(List<HeroSO> heroes)
    {
        if (heroes == null) return null;

        List<Hero> result = heroes.Select(h => CreateHero(h)).ToList();
        result = result.OrderBy(h => (int)h.HeroClass).ToList();
        return result;
    }

    private Hero CreateHero(HeroSO so)
        => (Hero)CreateCharacter<Hero>(so, _heroContainer, "_Hero");
    #endregion

    #region internal operations enemies
    public List<Enemy> CreateEnemies(List<EnemySO> enemies)
    {
        if (enemies == null) return null;

        return enemies.Select(h => CreateEnemy(h)).ToList();
    }

    public Enemy CreateEnemy(EnemySO so)
        => (Enemy)CreateCharacter<Enemy>(so, _enemyContainer, "_Enemy");

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
                _enemies.Add(enemy);
                _reinforcementEnemies.Remove(enemy);
                RefreshEnemiesData();
                OnCharacterEnterScene?.Invoke(enemy);
            }

        return true;
    }

    private void CheckEnemiesLines()
    {
        if (_enemies == null || _enemies.Count == 0) return;

        bool onBackline = _enemies[0].OnBackline;
        foreach (Enemy enemy in _enemies)
            if (enemy.OnBackline != onBackline)
            { 
                _enemiesInTwoLines = true;
                return;
            }
        
        _enemiesInTwoLines = false;
    }

    private void RefreshEnemiesData()
    {
        if (CalculateCharSizePool(_enemies) > MaxPoolEnemySize)
        {
            // make full reset from GameObjects
        }

        _currentPoolEnemySize = CalculateCharSizePool(_enemies);
        CheckEnemiesLines();
    }
    #endregion

    #region event hanlers
    private void OnCharacterChangedHandler(Character character)
    {
        CheckEnemiesLines();

        if (character.CurrentHealth > 0)
            OnCharacterChanged?.Invoke(character);
        else
        {
            if (character is Hero)
                HeroDies(character as Hero);
            else
                EnemyDies(character as Enemy);
        }
    }
    #endregion
}
