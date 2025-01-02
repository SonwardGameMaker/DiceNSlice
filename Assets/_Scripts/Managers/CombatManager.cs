using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Build;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    private const int MAX_PULL_CHARACTER_SIZE = 10;

    #region fields
    [SerializeField] private UiManager _uiManager;

    private List<Hero> _heroes;
    private List<Enemy> _activeEnemies;
    private List<Enemy> _reinforcementsEnemies;
    private int _activeEnemiesPulSize;

    private List<GameAction> _heroesActionOrder;
    #endregion

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
