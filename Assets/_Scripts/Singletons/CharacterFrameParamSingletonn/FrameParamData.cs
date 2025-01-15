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
    public int MainFrameHeight { get => _height; }
    public int SquareComponentsSide { get => _sideLength; }
    public int SquareComponentsPadding { get => _position; }
    #endregion
}
