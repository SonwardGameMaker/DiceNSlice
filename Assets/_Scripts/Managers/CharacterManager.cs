using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    private const int MAX_PULL_CHARACTER_SIZE = 10;

    [SerializeField] private UiManager uiManager;

    [Header("Containers")]
    [SerializeField] private Transform _heroContainer;
    [SerializeField] private Transform _enemyContainer;

    private List<Hero> _heroes;
    private List<Enemy> _activeEnemies;
    private List<Enemy> _reinforcementsEnemies;
    private int _activeEnemiesPulSize;

    #region init
    public void Setup()
    {
        _activeEnemiesPulSize = 0;
    }

    public void Setup(List<Hero> heroes, List<Enemy> enemies)
    {
        _heroes = heroes;

        if (CalculateCharSizePull(enemies) <= MAX_PULL_CHARACTER_SIZE)
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

    #region external interactions
    public List<Hero> GetHeroes()
    {
        //if (_heroContainer.childCount == 0)
        //    return CreateHeroes();

        List<Hero> result = new List<Hero>();
        for (int i = 0; i < _heroContainer.childCount; i++)
            result.Add(_heroContainer.GetChild(i).GetComponent<Hero>());
        return result;
    }

    public List<Enemy> GetEnemies()
    {
        //if (_enemyContainer.childCount == 0)
        //    return CreateEnemies();

        List<Enemy> result = new List<Enemy>();
        for (int i = 0; i < _heroContainer.childCount; i++)
            result.Add(_enemyContainer.GetChild(i).GetComponent<Enemy>());
        return result;
    }
    #endregion

    #region internal operations
    private List<Hero> CreateHeroes(List<HeroSO> heroes)
        => heroes.Select(h => CreateHero(h)).ToList();

    private Hero CreateHero(HeroSO so)
        => CreateCharacter(so) as Hero;

    private List<Enemy> CreateEnemies(List<EnemySO> enemies)
        => enemies.Select(e => CreateEnemy(e)).ToList();

    private Enemy CreateEnemy(EnemySO so)
        => CreateCharacter(so) as Enemy;

    private Character CreateCharacter(CharacterSO so)
    {
        string nameSuffix;
        Type type;

        if (so is HeroSO)
        {
            nameSuffix = "_Hero";
            type = typeof(Hero);
        }
        else
        {
            nameSuffix = "_Enemy";
            type = typeof(Enemy);
        }

        GameObject character = new GameObject(so.name + nameSuffix);
        Character result = character.AddComponent(type) as Character;
        character.AddComponent<Dice>();
        result.Setup(so);

        return result;
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
        while (activeSizePull <= MAX_PULL_CHARACTER_SIZE)
        {
            activeEnemies.Add(enemies[count]);
            activeSizePull += enemies[count].CharacterSize;
            count++;
        }

        for (int i = count; i < enemies.Count; i++)
            reinforcementsEnemy.Add(enemies[i]);

        return new(activeEnemies, reinforcementsEnemy);
    }

    private int CalculateCharSizePull<T>(List<T> characters) where T : Character
        => characters.Sum(c => c.CharacterSize);
    #endregion
}
