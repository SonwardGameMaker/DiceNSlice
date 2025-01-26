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
    #endregion

    #region properties
    public string Name => _name;
    public Sprite Sprite => _sprite;
    public GameAction GameAction => _gameAction;
    public int CurrentPips => _pips.CurrentValue;
    #endregion

    #region init
    public GameActionContainer(string name, Sprite sprite, GameAction gameAction)
    {
        _name = name;
        _sprite = sprite;
        _gameAction = gameAction;
        _pips = new ModVar(gameAction.BasePips);
        _baseKeywords = new List<Keyword>(); // TODO
    }
    #endregion
}
