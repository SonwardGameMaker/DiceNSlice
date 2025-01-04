using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    private const int MAX_POOL_CHARACTER_SIZE = 10;

    #region fields
    [SerializeField] private UiManager _uiManager;

    [Header("Containers")]
    [SerializeField] private Transform _heroContainer;
    [SerializeField] private Transform _enemyContainer;

    // heroes
    private List<Hero> _heroes;
    private List<Hero> _activeHeroes;
    private List<Hero> _deadHeroes;
    // enemies
    private List<Enemy> _enemies;
    private List<Enemy> _activeEnemies;
    private List<Enemy> _deadEnemies;
    private List<Enemy> _reinforcementsEnemies;
    #endregion

    #region events
    public event Action<Character> OnCharacterCreated;
    public event Action<Character> OnCharacterDeleted;
    public event Action<Character> OnCharacterChanged;
    #endregion

    #region init
    public void Setup(List<HeroSO> heroes, List<EnemySO> enemies)
        => Setup(CreateHeroes(heroes), CreateEnemies(enemies));

    public void Setup(List<Hero> heroes, List<Enemy> enemies)
    {
        _heroes = heroes;

        if (CalculateCharSizePool(enemies) <= MAX_POOL_CHARACTER_SIZE)
        {
            _activeEnemies = enemies;
            _reinforcementsEnemies = new List<Enemy>();
        }
        else
        {
            (List<Enemy>, List<Enemy>) result = SplitIntoReinforcements(enemies);
            _activeEnemies = result.Item1;
            _reinforcementsEnemies = result.Item2;
        }

    }
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

    // TODO
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
        while (activeSizePull <= MAX_POOL_CHARACTER_SIZE)
        {
            AddToActive();
            count++;
        }

        for (int i = count; i < enemies.Count; i++)
            reinforcementsEnemy.Add(enemies[i]);

        while (activeSizePull <= MAX_POOL_CHARACTER_SIZE && count < enemies.Count)
        {
            if (activeSizePull + enemies[count].CharacterSize <= MAX_POOL_CHARACTER_SIZE)
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
            activeSizePull += enemies[count].CharacterSize;
        }
    }

    private int CalculateCharSizePool<T>(List<T> characters) where T : Character
        => characters.Sum(c => c.CharacterSize);

    private List<Character> CreateCharacters(List<CharacterSO> characters)
        => characters.Select(character => CreateCharacter(character)).ToList();

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

        return false;
    }
    #endregion

    #region event hanlers
    private void OnCharacterChangedHandler(Character character)
    {
        OnCharacterChanged?.Invoke(character);
    }
    #endregion
}
