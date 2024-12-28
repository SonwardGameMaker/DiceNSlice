using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : Character
{
    private Item _item1;
    private Item _item2;

    public Item Item1 { get => _item1; }
    public Item Item2 { get => _item2; }
}
