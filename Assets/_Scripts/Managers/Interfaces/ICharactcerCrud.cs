using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharactcerCrud<T> where T : Character
{
    public event Action<T> CharacterAdded;
    public event Action<T> CharacterChanged;

    public void AddCharatcer(T character);
}
