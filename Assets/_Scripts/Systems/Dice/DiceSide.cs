using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class DiceSide : GameAction
{
    private List<Keyword> _addedKeywords;
    private List<Keyword> _currentKeywords;

    public DiceSide(string name, Sprite sprite, int pips, List<Keyword> baseKeywords)
    {
        _name = name;
        _sprite = sprite;
        _pips = new ModVar(pips);
        _baseKeywords = baseKeywords;
        _addedKeywords = new List<Keyword>();
        ResetKeywords();
    }
    public DiceSide(string name, Sprite sprite, int pips)
    {
        _name = name;
        _sprite = sprite;
        _pips = new ModVar(pips);
        _baseKeywords = new List<Keyword>();
        _addedKeywords = new List<Keyword>();
        ResetKeywords();
    }

    public void ResetKeywords()
        => _currentKeywords = new List<Keyword>(_baseKeywords);
}
