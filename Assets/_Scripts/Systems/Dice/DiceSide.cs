using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class DiceSide
{
    private string _name;
    private Sprite _sprite;
    private ModVar _pips;
    private List<Keyword> _baseKeywords;
    private List<Keyword> _currentKeywords;

    public DiceSide(string name, Sprite sprite, int pips, List<Keyword> baseKeywords)
    {
        _name = name;
        _sprite = sprite;
        _pips = new ModVar(pips);
        _baseKeywords = baseKeywords;
        ResetKeywords();
    }
    public DiceSide(string name, Sprite sprite, int pips)
    {
        _name = name;
        _sprite = sprite;
        _pips = new ModVar(pips);
        _baseKeywords = new List<Keyword>();
        ResetKeywords();
    }

    public abstract List<Character> Activate(List<Character> list);

    public abstract List<Character> GetValidTargets(List<Character> list);

    public void ResetKeywords()
        => _currentKeywords = new List<Keyword>(_baseKeywords);

    public abstract void UseOn(Character target);
}
