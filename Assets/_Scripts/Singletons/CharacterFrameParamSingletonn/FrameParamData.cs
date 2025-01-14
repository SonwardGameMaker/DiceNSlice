using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class FrameParamData
{
    [SerializeField] private Vector2 _mainFrameParams;
    [SerializeField] private Vector2 _portraitFrameParams;
    [SerializeField] private Vector2 _diceCellFrameParams;

    public Vector2 MainFrameParams { get => _mainFrameParams; }
    public Vector2 PortraitFrameParams { get => _portraitFrameParams; }
    public Vector2 DiceCellFrameParams { get => _diceCellFrameParams; }
}
