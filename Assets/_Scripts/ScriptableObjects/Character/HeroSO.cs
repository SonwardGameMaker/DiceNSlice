using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObects/CharacterData/HeroSO")]
public class HeroSO : CharacterSO
{
    #region fields
    [SerializeField] protected Item _item1;
    [SerializeField] protected Item _item2;
    [SerializeField] protected HeroClass _heroClass;
    [SerializeField] protected int _heroLevel;
    #endregion

    #region init
    public HeroSO() : base() { }

    public HeroSO(string name, Sprite portrait, int maxHealth, int currentHealth, CharacterSize characterSize, Item item1, Item item2, HeroClass heroClass, int heroLevel)
        : base(name, portrait, maxHealth, currentHealth, characterSize)
    {
        _item1 = item1;
        _item2 = item2;
        _heroClass = heroClass;
        _heroLevel = heroLevel;
    }
    #endregion

    #region properties
    public Item Item1 => _item1;
    public Item Item2 => _item2;
    public HeroClass HeroClass => _heroClass;
    public int HeroLevel => _heroLevel;
    #endregion
}
