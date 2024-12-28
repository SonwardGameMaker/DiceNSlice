using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModVar
{
    #region fields
    private int _baseValue;
    private int _currentValue;

    private List<Modifier> _additiveModifiers;
    private List<Modifier> _multiplicativeModifiers;
    #endregion

    #region events
    public event Action OnValueChanged;
    #endregion

    #region init
    public ModVar(int baseValue)
    {
        _baseValue = baseValue;
        _currentValue = baseValue;

        _additiveModifiers = new List<Modifier>();
        _multiplicativeModifiers = new List<Modifier>();
    }
    #endregion

    #region properties
    public int BaseValue
    {
        get => _baseValue;
        set
        {
            _baseValue = value;
            CalculateCurrentValue();
        }
    }

    public int CurrentValue { get => _currentValue; }
    #endregion

    #region external interactions
    public bool AddModifier(Modifier modifier)
    {
        if (modifier.Type == ModifierType.Additive)
            _additiveModifiers.Add(modifier);
        else
            _multiplicativeModifiers.Add(modifier);

        CalculateCurrentValue();
        return true; //TODO Make method try add in modifiers, that will define adding behaviour for each modifier class
    }
    #endregion

    #region internal operations
    private void CalculateCurrentValue()
    {
        _currentValue = _baseValue;

        foreach (Modifier modifier in _additiveModifiers)
            _currentValue += modifier.Value;
        foreach(Modifier modifier in _multiplicativeModifiers)
            _currentValue *= modifier.Value;

        ValueChanged();
    }
    #endregion

    #region event triggers
    private void ValueChanged()
        => OnValueChanged?.Invoke();
    #endregion
}
