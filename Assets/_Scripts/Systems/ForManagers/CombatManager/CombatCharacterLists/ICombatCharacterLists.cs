using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICombatCharacterLists
{
    #region events
    public event Action<Character> OnCharacterEnterScene;
    public event Action<Character> OnCharacterLeaveScene;
    public event Action<Character, bool> OnCharacterCreated;
    public event Action<Character> OnCharacterDeleted;
    public event Action<Character> OnCharacterChanged;
    #endregion

    #region properties
    public List<Hero> PresentHeroes { get; }
    public List<Hero> DeadHeroes { get; }

    public List<Enemy> PresentEnemies { get; }
    public List<Enemy> DeadEnemies { get; }
    public List<Enemy> EnemyReinforcements { get; }

    public int CurrentEnemyPoolSize { get; }
    #endregion

    #region external interactions heroes
    public void AddHeroes(List<Hero> heroes);
    public void AddHero(Hero hero);
    public void RemoveHero(Hero hero);
    public void HeroDies(Hero hero);
    #endregion

    #region external interactions enemies
    public void AddEnemies(List<Enemy> enemies);
    public void AddEnemy(Enemy enemy);
    public void RemoveEnemy(Enemy enemy);
    public void MoveEnemyToReinforcements(Enemy enemy, List<Enemy> enemyList = null);
    public void EnemyDies(Enemy enemy);
    #endregion

    #region event handlers
    public void OnCharacterChangedHandler(Character character);
    #endregion
}
