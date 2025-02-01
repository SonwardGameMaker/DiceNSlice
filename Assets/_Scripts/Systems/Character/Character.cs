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
    [SerializeField] protected int _shields;
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

        _maxHealth.OnValueChanged += OnCharacterChangedTrigger;
    }

    private void OnDestroy()
    {
        _maxHealth.OnValueChanged -= OnCharacterChangedTrigger;
    }
    #endregion

    #region properties
    public string Name => _name;
    public Sprite Portrait => _portrait;
    public int MaxHealth { get => _maxHealth.CurrentValue; }
    public int MaxHealthBaseValue { get => _maxHealth.BaseValue; set => SetMaxHp(value); }
    public int CurrentHealth { get => _currentHealth; set => SetHp(value); }
    public CharacterSize CharacterSize => _characterSize;
    public int Shields { get => _shields; set => SetShields(value); }
    public StatusEffectSystem StatusEffectManager => StatusEffectManager;
    public Dice Dice => _dice;
    #endregion

    #region external interactions
    public void TakeDamage(int damage)
    {
        damage = ApplyDamageTo(ref _shields, damage);
        ApplyDamageTo(ref _currentHealth, damage);
        OnCharacterChangedTrigger();
    }

    public void ChangeHp(int amount)
        => SetValue(ref _currentHealth, Math.Min(MaxHealth, _currentHealth + amount));

    public void ChangeShields(int amount)
        => SetValue(ref _shields, Math.Max(0, _shields + amount));

    public void AddHpModifier(Modifier modifier)
        => _maxHealth.AddModifier(modifier);

    public void RemoveHpModifier(Modifier modifier)
        => _maxHealth.RemoveModifier(modifier);

    public virtual void ResetCharacter()
    {
        // TODO: Reset Status Effects
        
    }

    public void ResetShields()
        => SetShields(0);
    #endregion

    #region internal operations
    protected void SetMaxHp(int amount)
    {
        if (amount < 0) throw new Exception("Value canot be less then zero");
        _maxHealth.BaseValue = amount;

        OnCharacterChangedTrigger();
    }

    protected void SetHp(int amount)
    {
        if (amount > _maxHealth.CurrentValue) throw new Exception("Current Health cannot be bigger than Max Health");
        SetValue(ref _currentHealth, amount);
    }

    protected void SetShields(int amount)
    {
        if (amount < 0) throw new Exception("Value canot be less then zero");
        SetValue(ref _shields, Math.Max(0, amount));
    }

    protected void SetValue(ref int target,  int value)
    {
        target = value;
        OnCharacterChangedTrigger();
    }

    private int ApplyDamageTo(ref int target, int damage)
    {
        if (target <= 0) return damage;

        int absorbedDamage = Mathf.Min(target, damage);
        target -= absorbedDamage;

        return damage - absorbedDamage;
    }
    #endregion

    #region event triggers
    protected void OnCharacterChangedTrigger()
        => OnCharacterChanged?.Invoke(this);
    #endregion
}
