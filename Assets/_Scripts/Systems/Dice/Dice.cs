using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    #region fields
    private Character _owner;
    private DiceSide[] _sides;
    private DiceSide _rolledSide;
    private bool _isLocked;
    
    private System.Random _rand;
    #endregion

    #region init
    public void Setup(Character owner, DiceSide[] side = null)
    {
        _owner = owner;

        if (side == null || side.Length > 6)
        {
            Debug.LogError("Sides is not set properly");
            _sides = new DiceSide[6];
        }
        
        _rolledSide = null;
        _isLocked = false;
    }
    #endregion

    #region properties
    public Character Owner { get => _owner; }
    public DiceSide[] Sides { get => _sides; }
    public DiceSide RolledDice { get => _rolledSide; }
    public bool IsLocked { get => _isLocked; }
    #endregion

    #region external interactions
    public DiceSide RollTheDice()
    {
        DiceSide rolledSide = _sides[_rand.Next(0,5)];
        _rolledSide = rolledSide;
        return rolledSide;
    }

    public void LockTheDice() => _isLocked = true;

    public void UnlockTheDice() => _isLocked = false;

    public bool ValidateSides()
    {
        throw new NotImplementedException();
    }
    #endregion
}
