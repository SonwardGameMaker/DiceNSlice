using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : Character
{
    #region fields
    [SerializeField] private HeroClass _heroClass;

    private int _heroLevel;
    private Item _item1;
    private Item _item2;
    #endregion

    #region init
    public override void Setup(CharacterSO so)
    {
        if (so is HeroSO)
            Setup(so as HeroSO);
        else
            base.Setup(so);
    }

    public void Setup(HeroSO so)
    {
        base.Setup(so);
        _heroClass = so.HeroClass;
        _heroLevel = so.HeroLevel;
        _item1 = so.Item1;
        _item2 = so.Item2;
    }
    #endregion

    #region properties
    public HeroClass HeroClass { get => _heroClass; }
    public int HeroLevel { get => _heroLevel; }
    public Item Item1 { get => _item1; set => SetItem(value, ref _item1); }
    public Item Item2 { get => _item2; set => SetItem(value, ref _item2); }
    #endregion

    #region internal operations
    private void SetItem(Item item, ref Item slot)
    {
        slot = item;

        // TODO
    }
    #endregion
}
