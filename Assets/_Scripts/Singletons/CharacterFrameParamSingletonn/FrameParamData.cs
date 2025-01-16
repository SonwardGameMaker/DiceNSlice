using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class FrameParamData
{
    #region fields
    [Header("Main frame")]
    [SerializeField] private int _height;

    [Header("Square Components")]
    [SerializeField] private int _sideLength;
    [SerializeField] private int _position;
    #endregion

    #region properties
    public int MainFrameHeight => _height;
    public int SquareComponentsSide => _sideLength;
    public int SquareComponentsPadding => _position;
    #endregion
}
