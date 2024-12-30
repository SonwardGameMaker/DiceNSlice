using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    #region fields
    [SerializeField] protected string _name;
    [SerializeField] protected ModVar _maxHealth;
    [SerializeField] protected int _currentHealth;
    protected int _shields;
    protected StatusEffectSystem _statusEffectSystem;
    protected Dice _dice;
    #endregion

    #region init
    public void Setup()
    {
        _shields = 0;

        _dice = GetComponent<Dice>();
        _dice.Setup(this);

        _statusEffectSystem = GetComponent<StatusEffectSystem>();
    }

    public void Setup(CharacterSO so)
    {
        _name = so.name;
        _maxHealth = new ModVar(so.MaxHealth);
        _currentHealth = so.CurrentHealth;

        Setup();
    }
    #endregion

    #region properties
    public string Name { get => _name; }
    public int MaxHealth { get => _maxHealth.CurrentValue; }
    public int MaxHealthBaseValue { get => _maxHealth.BaseValue; set => SetMaxHp(value); }
    public int CurrentHealth { get => _currentHealth; set => SetHp(value); }
    public int Shields { get => _shields; set => SetShields(value); }
    public StatusEffectSystem StatusEffectManager { get => StatusEffectManager; }
    public Dice Dice { get => _dice; }
    #endregion

    #region internal operations
    protected void SetMaxHp(int amount)
    {
        if (amount < 0) throw new Exception("Value canot be less then zero");
        _maxHealth.BaseValue = amount;
    }
    protected void SetHp(int amount)
    {
        if (amount > _maxHealth.CurrentValue) throw new Exception("Current Health cannot be bigger than Max Health");
        SetValue(ref _currentHealth, amount);
    }
    protected void SetShields(int amount)
    {
        SetValue(ref _shields, amount);
    }

    protected void SetValue(ref int target,  int value)
    {
        if (value < 0) throw new Exception("Value canot be less then zero");
        target = value;
    }
    #endregion
}
