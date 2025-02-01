using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameActionContainer
{
    #region fields
    protected string _name;
    protected Sprite _sprite;
    protected GameAction _gameAction;
    protected ModVar _pips;
    protected List<Keyword> _baseKeywords;
    protected bool _enabled;
    #endregion

    #region events
    public event Action OnActionContainerChanged;
    #endregion

    #region init
    public GameActionContainer(string name, Sprite sprite, GameAction gameAction)
    {
        _name = name;
        _sprite = sprite;
        _gameAction = gameAction;
        _pips = new ModVar(gameAction.BasePips);
        _baseKeywords = new List<Keyword>(); // TODO
        _enabled = true;

        gameAction.OnActionChanged += OnInternalChangesHandler;
        _pips.OnValueChanged += OnInternalChangesHandler;

        gameAction.OnActionUsed += OnActionUsedHandler;
    }

    ~GameActionContainer()
    {
        _gameAction.OnActionChanged -= OnInternalChangesHandler;
        _pips.OnValueChanged -= OnInternalChangesHandler;

        _gameAction.OnActionUsed -= OnActionUsedHandler;
    }
    #endregion

    #region properties
    public string Name => _name;
    public Sprite Sprite => _sprite;
    public GameAction GameAction => _gameAction;
    public int CurrentPips => _pips.CurrentValue;
    public bool Enabled
    {
        get => _enabled;
        set
        {
            _enabled = value;
            OnActionContainerChanged?.Invoke();
        }
    }
    #endregion

    #region event handlers
    protected virtual void OnInternalChangesHandler()
        => OnActionContainerChanged?.Invoke();

    protected virtual void OnActionUsedHandler()
        => Enabled = false;
    #endregion
}
