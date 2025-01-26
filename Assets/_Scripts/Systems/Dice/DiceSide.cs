using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[Serializable]
public class DiceSide : GameActionContainer
{
    #region fields
    private List<Keyword> _addedKeywords;
    private List<Keyword> _currentKeywords;
    #endregion

    #region init
    public DiceSide(string name, Sprite sprite, GameAction gameAction, List<Keyword> baseKeywords)
        : base(name, sprite, gameAction)
    {
        _baseKeywords = baseKeywords;
        _addedKeywords = new List<Keyword>();
        ResetKeywords();
    }
    public DiceSide(string name, Sprite sprite, GameAction gameAction)
        : base(name, sprite, gameAction)
    {
        _baseKeywords = new List<Keyword>();
        _addedKeywords = new List<Keyword>();
        ResetKeywords();
    }
    #endregion

    #region properties
    public List<Keyword> AddedKeywords => _addedKeywords;
    public List<Keyword> CurentKeywords => _currentKeywords;
    #endregion

    #region external interactions
    public void AddKeyword(Keyword keyword)
    {
        throw new NotImplementedException();
    }

    public void RemoveKeyword(Keyword keyword)
    {
        throw new NotImplementedException();
    }

    public void ResetKeywords()
        => _currentKeywords = new List<Keyword>(_baseKeywords);
    #endregion
}
