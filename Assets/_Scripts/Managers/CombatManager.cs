using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    #region fields
    private List<GameAction> _heroesActionOrder;

    // temp
    private Hero _activeHero;
    #endregion

    #region events
    public event Action<Hero> OnHeroActivated;
    public event Action<Hero> OnHeroDeactivated;
    #endregion

    #region init
    public void Setup()
    {
        _activeHero = null;
        // TODO
    }
    #endregion

    #region properties

    // temp
    public Hero ActiveHero { get => _activeHero; }
    #endregion

    #region external interactions
    public void SelectCharacter(Character character)
    {
        // temp
        Debug.Log($"{typeof(CharacterManager)} - {nameof(SelectCharacter)}");
        if (character is Hero hero)
        {
            ActivateHero(hero);
        }
    }

    public void Cancel()
    {

        // temp
        Debug.Log($"{typeof(CharacterManager)} - {nameof(Cancel)}");
        DeactivateHero();
    }
    #endregion

    #region internal operactions
    private void ActivateHero(Hero hero)
    {
        if (_activeHero == null)
        {
            _activeHero = hero;
            OnHeroActivated?.Invoke(hero);
        }
    }

    private void DeactivateHero()
    {
        if (_activeHero != null)
        {
            OnHeroDeactivated?.Invoke(_activeHero);
            _activeHero = null;
        }
    }
    #endregion
}
