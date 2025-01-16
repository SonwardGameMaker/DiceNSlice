using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObects/CharacterData/HeroSO")]
public class HeroSO : CharacterSO
{
    #region fields
    [SerializeField] Item _item1;
    [SerializeField] Item _item2;
    [SerializeField] HeroClass _heroClass;
    [SerializeField] int _heroLevel;
    #endregion

    #region properties
    public Item Item1 => _item1;
    public Item Item2 => _item2;
    public HeroClass HeroClass => _heroClass;
    public int HeroLevel => _heroLevel;
    #endregion
}
