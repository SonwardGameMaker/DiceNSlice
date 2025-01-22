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
    [SerializeField] protected DiceSide _leftSide;
    [SerializeField] protected DiceSide _middleSide;
    [SerializeField] protected DiceSide _topSide;
    [SerializeField] protected DiceSide _bottomSide;
    [SerializeField] protected DiceSide _rightSide;
    [SerializeField] protected DiceSide _rightmostSide;
    #endregion

    #region init
    public CharacterSO() { }
    public CharacterSO(string name, Sprite portrait, int maxHealth, int currentHealth, CharacterSize characterSize)
    {
        _name = name;
        _portrait = portrait;
        _maxHealth = maxHealth;
        _currentHealth = currentHealth;
        _characterSize = characterSize;
    }
    #endregion

    #region properties
    public string Name => _name;
    public Sprite Portrait => _portrait;
    public int MaxHealth => _maxHealth;
    public int CurrentHealth => _currentHealth;
    public CharacterSize CharacterSize => _characterSize;

    public DiceSide LeftSide => _leftSide;
    public DiceSide MiddleSide => _middleSide;
    public DiceSide TopSide => _topSide;
    public DiceSide BottomSide => _bottomSide;
    public DiceSide RightSide => _rightSide;
    public DiceSide RightmostSide => _rightmostSide;
    #endregion

    #region external interactions
    public DiceSide[] GetDiceSides()
    {
        DiceSide[] result = new DiceSide[6];
        result[0] = _leftSide;
        result[1] = _middleSide;
        result[2] = _topSide;
        result[3] = _bottomSide;
        result[4] = _rightSide;
        result[5] = _rightmostSide;

        return result;
    }
    #endregion
}
