using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameActionContainer
{
    protected string _name;
    protected Sprite _sprite;
    protected ModVar _pips;
    protected List<Keyword> _baseKeywords;

    public abstract List<Character> GetValidTargets(List<Character> list);
    public abstract void UseOn(Character target);
}
