using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    #region events
    public event Action<Dice> OnDiceChanged;
    #endregion

    #region init
    public void Setup(Character owner, List<DiceSide> sides = null)
    {
        _owner = owner;
        _rand = new System.Random();

        if (sides == null || sides.Count > 6)
        {
            Debug.LogError("Sides is not set properly");
            _sides = new DiceSide[6];
        }
        
        _sides = sides.ToArray();

        _rolledSide = null;
        _isLocked = false;

        foreach (DiceSide side in _sides)
            side.OnActionContainerChanged += OnInternalDataChangesHandler;
    }

    private void OnDestroy()
    {
        foreach (DiceSide side in _sides)
            side.OnActionContainerChanged -= OnInternalDataChangesHandler;
    }
    #endregion

    #region properties
    public Character Owner => _owner;
    public DiceSide[] Sides => _sides;
    public DiceSide RolledSide => _rolledSide;
    public bool IsLocked 
    {
        get => _isLocked;
        set 
        {
            _isLocked = value;
            OnInternalDataChangesHandler();
        } 
    }
    #endregion

    #region external interactions
    public DiceSide RollTheDice()
    {
        DiceSide rolledSide = _sides[_rand.Next(0,5)];
        _rolledSide = rolledSide;
        return rolledSide;
    }

    public bool SetRolledSide(DiceSide side)
    {
        if (_sides.Contains(side))
        {
            _rolledSide = side;
            return true;
        }

        return false;
    }

    public bool SetLockedSide(DiceSide side)
    {
        bool result = SetRolledSide(side);
        if (result)
            IsLocked = true;

        return result;
    }
    #endregion

    #region event handlers
    private void OnInternalDataChangesHandler()
        => OnDiceChanged?.Invoke(this);
    #endregion
}
