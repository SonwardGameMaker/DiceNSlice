using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObects/CharacterData/DiceSO")]
public class DiceSO : ScriptableObject
{
    [Header("LeftSide")]
    [SerializeField] private DiceSideSO _leftSide;
    [SerializeField] private int _leftSidePips;

    [Header("MiddleSide")]
    [SerializeField] private DiceSideSO _middleSide;
    [SerializeField] private int _middleSidePips;

    [Header("TopSide")]
    [SerializeField] private DiceSideSO _topSide;
    [SerializeField] private int _topSidePips;

    [Header("BottomSide")]
    [SerializeField] private DiceSideSO _bottomSide;
    [SerializeField] private int _bottomSidePips;

    [Header("RightSide")]
    [SerializeField] private DiceSideSO _rightSide;
    [SerializeField] private int _rightSidePips;

    [Header("RightmostSide")]
    [SerializeField] private DiceSideSO _rightmostSide;
    [SerializeField] private int _rightmostSidePips;


    public List<DiceSide> GetDiceSides()
    {
        List<DiceSide> diceSides = new List<DiceSide>();
        diceSides.Add(CreateDiceSide(_leftSide, _leftSidePips));
        diceSides.Add(CreateDiceSide(_middleSide, _middleSidePips));
        diceSides.Add(CreateDiceSide(_topSide, _topSidePips));
        diceSides.Add(CreateDiceSide(_bottomSide, _bottomSidePips));
        diceSides.Add(CreateDiceSide(_rightSide, _rightSidePips));
        diceSides.Add(CreateDiceSide(_rightmostSide, _rightmostSidePips));

        return diceSides;
    }

    private DiceSide CreateDiceSide(DiceSideSO side, int pips)
    {
        GameAction gameAction;
        switch (side.GameActionType)
        {
            case GameActionType.Damage:
                gameAction = new DamageAction(pips);
                break;
            case GameActionType.DamageAll:
                gameAction = new DamageAllAction(pips);
                break;
            case GameActionType.Heal:
                gameAction = new HealAction(pips);
                break;
            case GameActionType.HealAll:
                gameAction = new HealAllAction(pips);
                break;
            case GameActionType.Shield:
                gameAction = new ShieldAction(pips);
                break;
            case GameActionType.ShieldAll:
                gameAction = new ShieldAllAction(pips);
                break;
            default: return null;
        }

        return new DiceSide(side.Name, side.Sprite, gameAction);
    }
}
