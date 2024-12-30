using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : Character
{
    [SerializeField] private HeroClass _heroClass;
    private Item _item1;
    private Item _item2;

    public HeroClass HeroClass { get => _heroClass; }
    public Item Item1 { get => _item1; set => SetItem(value, ref _item1); }
    public Item Item2 { get => _item2; set => SetItem(value, ref _item2); }

    private void SetItem(Item item, ref Item slot)
    {
        slot = item;

        // TODO
    }
}
