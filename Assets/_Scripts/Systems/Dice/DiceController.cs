using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class DiceController : MonoBehaviour
{
    #region fields
    [SerializeField] private Dice _dice;
    
    private DiceSide _rolledSide;
    private bool _isLocked;

    private System.Random _rand;
    #endregion

    #region init
    public void Setup(Dice dice, bool enabled = true)
    {
        _dice = dice;

        Enabled = enabled;

        _rolledSide = null;
        _isLocked = false;

        _rand = new System.Random();
    }
    #endregion

    #region properties
    public bool Enabled { get; set; }
    public Dice Dice => _dice;
    public DiceSide RolledSide => _rolledSide;
    public bool IsLocked => _isLocked;
    #endregion

    #region external interactions
    public void RollTheDice()
    {
        // TODO: make it throught gameObject dices ---------
        DiceSide rolledSide = _dice.Sides[_rand.Next(0, 5)];
        // -------------------------------------------------

        _dice.SetRolledSide(rolledSide);
    }

    public void LockTheDice()
    {
        _dice.IsLocked = true;
        _isLocked = true;
    }

    public void UnlockTheDice()
    {
        _dice.IsLocked = false;
        _isLocked = false;
    }

    public void EnableDices()
    {
        foreach (DiceSide diceSide in _dice.Sides)
            if (!diceSide.Enabled)
                diceSide.Enabled = true;
    }
    #endregion
}
