using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    #region fields
    [SerializeField] protected string _name;
    [SerializeField] protected Sprite _portrait;
    [SerializeField] protected ModVar _maxHealth;
    [SerializeField] protected int _currentHealth;
    [SerializeField] protected CharacterSize _characterSize;
    protected int _shields;
    protected StatusEffectSystem _statusEffectSystem;
    protected Dice _dice;
    #endregion

    #region events
    public event Action<Character> OnCharacterChanged;
    #endregion

    #region init
    public virtual void Setup()
    {
        _dice = GetComponent<Dice>();
        _dice.Setup(this);

        Init();
    }

    public virtual void Setup(CharacterSO so)
    {
        _name = so.Name;
        _portrait = so.Portrait;
        _maxHealth = new ModVar(so.MaxHealth);
        _currentHealth = so.CurrentHealth;
        _characterSize = so.CharacterSize;

        _dice = GetComponent<Dice>();
        _dice.Setup(this, so.GetDiceSides());

        Init();
    }

    private void Init()
    {
        _shields = 0;

        _statusEffectSystem = GetComponent<StatusEffectSystem>();

        _maxHealth.OnValueChanged += OnCharacterChanegtTrigger;
    }

    private void OnDestroy()
    {
        _maxHealth.OnValueChanged -= OnCharacterChanegtTrigger;
    }
    #endregion

    #region properties
    public string Name { get => _name; }
    public Sprite Portrait { get => _portrait; }
    public int MaxHealth { get => _maxHealth.CurrentValue; }
    public int MaxHealthBaseValue { get => _maxHealth.BaseValue; set => SetMaxHp(value); }
    public int CurrentHealth { get => _currentHealth; set => SetHp(value); }
    public CharacterSize CharacterSize { get => _characterSize; }
    public int Shields { get => _shields; set => SetShields(value); }
    public StatusEffectSystem StatusEffectManager { get => StatusEffectManager; }
    public Dice Dice { get => _dice; }
    #endregion

    #region external interactions
    public void ChangeHp(int amount)
    {
        if (_currentHealth + amount > MaxHealth) _currentHealth = MaxHealth;
        else _currentHealth += amount;

        OnCharacterChanegtTrigger();
    }

    public void ChangeShields(int amount)
    {
        if (_shields + amount < 0) _shields = 0;
        else _shields += amount;

        OnCharacterChanegtTrigger();
    }

    public void AddHpModifier(Modifier modifier)
        => _maxHealth.AddModifier(modifier);

    public void RemoveHpModifier(Modifier modifier)
        => _maxHealth.RemoveModifier(modifier);

    public virtual void ResetCharacter()
    {
        // Reset Status Effects
        
    }
    #endregion

    #region internal operations
    protected void SetMaxHp(int amount)
    {
        if (amount < 0) throw new Exception("Value canot be less then zero");
        _maxHealth.BaseValue = amount;

        OnCharacterChanegtTrigger();
    }
    protected void SetHp(int amount)
    {
        if (amount > _maxHealth.CurrentValue) throw new Exception("Current Health cannot be bigger than Max Health");
        SetValue(ref _currentHealth, amount);

        OnCharacterChanegtTrigger();
    }
    protected void SetShields(int amount)
    {
        SetValue(ref _shields, amount);

        OnCharacterChanegtTrigger();
    }

    protected void SetValue(ref int target,  int value)
    {
        if (value < 0) throw new Exception("Value canot be less then zero");
        target = value;
    }
    #endregion

    #region event triggers
    protected void OnCharacterChanegtTrigger()
        => OnCharacterChanged?.Invoke(this);
    #endregion
}
