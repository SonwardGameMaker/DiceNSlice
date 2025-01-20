using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManagerCmts : MonoBehaviour
{
    #region events
    // Heroes
    public event Action OnCreateHeroClicked;
    public event Action OnHeroDiesClicked;
    public event Action OnHeroUndiesClicked;
    public event Action OnResetHeroClicked;

    // Emenies
    public event Action OnCreateEnemyClicked;
    public event Action OnEnemyDiesClicked;
    public event Action OnEnemyUndiesClicked;
    public event Action OnEnemyResetClicked;
    #endregion

    #region external interactions
    // Heroes
    public void CreateHero()
        => OnCreateHeroClicked?.Invoke();

    public void HeroDies()
        => OnHeroDiesClicked?.Invoke();

    public void HeroUndies()
        => OnHeroUndiesClicked?.Invoke();

    public void ResetHero()
        => OnResetHeroClicked?.Invoke();

    // Enemies
    public void CreateEnemy()
        => OnCreateEnemyClicked?.Invoke();

    public void EnemyDies()
        => OnEnemyDiesClicked?.Invoke();

    public void EnemyUndies()
        => OnEnemyUndiesClicked?.Invoke();

    public void ResetEnemy()
        => OnEnemyResetClicked?.Invoke();
    #endregion
}
