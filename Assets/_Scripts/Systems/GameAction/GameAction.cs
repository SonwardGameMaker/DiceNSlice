using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameAction
{
    #region fields
    protected string _name;
    protected string _description;
    protected Sprite _sprite;
    protected bool _usingPips;
    protected int _basePips;
    #endregion

    #region init
    public GameAction(string name, string description, Sprite sprite, bool usingPips, int basePips)
    {
        _name = name;
        _description = description;
        _sprite = sprite;
        _usingPips = usingPips;
        _basePips = basePips;
    }
    #endregion

    #region properties
    public string Name { get => _name; }
    public string Description { get => _description; }
    public Sprite Sprite { get => _sprite; }
    public bool UsingPips { get => _usingPips; }
    public int BasePips { get => _basePips; }
    #endregion

    #region external interactions
    /// <summary>
    /// This method identifies valid targets for the 'UsingOn' method
    /// </summary>
    /// <param name="alies"> Takes alies for character that using this method </param>
    /// <param name="enemies"> Takes enemies for character that using this method </param>
    /// <returns> Valid targets for the 'UsingOn' method </returns>
    public abstract List<Character> GetValidTargets(List<Hero> alies, List<Enemy> enemies);

    public abstract void UseOn(List<Character> characters);

    public abstract void UndoUsing(List<Character> characters);
    #endregion
}
