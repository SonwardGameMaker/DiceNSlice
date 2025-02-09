using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public interface ICombatManager
{
    #region events
    // States
    public event Action OnPreparingStateStarts;
    public event Action OnRollingStateStarts;

    // Characters
    public event Action<Hero> OnHeroActivated;
    public event Action<Hero> OnHeroDeactivated;

    // Combat flow
    /// <summary>
    /// bool - indicates if player won. true - won, false - lose
    /// </summary>
    public event Action<bool> OnCombatEnded;
    public event Action OnTurnEnded;
    #endregion

    #region init
    public void Setup(ICharacterManager charactcerManager);
    #endregion

    #region properties
    public CombatStateMachine StateMachine { get; }
    public ICombatCharacterLists CombatLists { get; }
    #endregion

    #region external interactions
    public void StartCombat();
    public void SelectCharacter(Character character);
    public void Next();
    public void Cancel();
    #endregion
}
