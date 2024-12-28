using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Keyword
{
    protected string _name;

    public Keyword()
    {
        _name = SetName();
    }

    public string Name { get => _name; }

    public abstract void Affect();

    protected abstract string SetName();
}
