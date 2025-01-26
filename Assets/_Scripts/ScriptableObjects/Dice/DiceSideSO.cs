using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObects/CharacterData/DiceSideSO")]
public class DiceSideSO : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private Sprite _sprite;
    [SerializeField] private GameActionType _gameActionType;

    public string Name => _name;
    public Sprite Sprite => _sprite;
    public GameActionType GameActionType => _gameActionType;
}
