using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSO : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private Sprite _portrait;
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _currentHealth;
    [SerializeField] private CharacterSize _characterSize;

    [Header("Dice Sides")]
    [SerializeField] private DiceSide _leftSide;
    [SerializeField] private DiceSide _middleSide;
    [SerializeField] private DiceSide _topSide;
    [SerializeField] private DiceSide _bottomSide;
    [SerializeField] private DiceSide _rightSide;
    [SerializeField] private DiceSide _rightmostSide;

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
}
