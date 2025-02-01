using System;
using System.Collections.Generic;

public abstract class GameAction
{
    #region fields
    protected string _name;
    protected string _description;
    protected bool _usingPips;
    protected int _basePips;

    protected List<Character> _validTargets;
    #endregion

    #region events
    public event Action OnActionChanged;
    public abstract event Action OnActionUsed;
    #endregion

    #region init
    public GameAction(int basePips, bool usingPips = true)
    {
        _usingPips = usingPips;
        _basePips = basePips;

        _validTargets = new List<Character>();
    }
    #endregion

    #region properties
    public string Name => _name;
    public string Description => _description;
    public bool UsingPips => _usingPips;
    public int BasePips => _basePips;
    #endregion

    #region external interactions
    /// <summary>
    /// This method identifies valid targets for the 'UsingOn' method
    /// </summary>
    /// <param name="alies"> Takes alies for character that using this method </param>
    /// <param name="enemies"> Takes enemies for character that using this method </param>
    /// <returns> Valid targets for the 'UsingOn' method </returns>
    public abstract List<Character> GetValidTargets(List<Character> allies, List<Character> enemies);
    #endregion
}
