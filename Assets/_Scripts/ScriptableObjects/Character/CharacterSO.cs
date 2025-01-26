using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSO : ScriptableObject
{
    #region fields
    [SerializeField] protected string _name;
    [SerializeField] protected Sprite _portrait;
    [SerializeField] protected int _maxHealth;
    [SerializeField] protected int _currentHealth;
    [SerializeField] protected CharacterSize _characterSize;

    [Header("Dice Sides")]
    [SerializeField] protected DiceSO _dice;
    #endregion

    #region properties
    public string Name => _name;
    public Sprite Portrait => _portrait;
    public int MaxHealth => _maxHealth;
    public int CurrentHealth => _currentHealth;
    public CharacterSize CharacterSize => _characterSize;
    #endregion

    #region external interactions
    public List<DiceSide> GetDiceSides()
        => _dice.GetDiceSides();
    #endregion
}
