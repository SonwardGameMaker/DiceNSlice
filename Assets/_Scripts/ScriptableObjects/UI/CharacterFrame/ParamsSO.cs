using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObects/UI/CharacterFrame/Params")]
public class ParamsSO : ScriptableObject
{
    #region fields
    [SerializeField] private FrameParamData _sizeOneParam;
    [SerializeField] private FrameParamData _sizeTwoParam;
    [SerializeField] private FrameParamData _sizeThreeParam;
    [SerializeField] private FrameParamData _sizeFourParam;
    [SerializeField] private FrameParamData _sizeFiveParam;
    [SerializeField] private FrameParamData _sizeSixParam;
    #endregion

    #region properties
    public FrameParamData SizeOneParam => _sizeOneParam;
    public FrameParamData SizeTwoParam => _sizeTwoParam;
    public FrameParamData SizeThreeParam => _sizeThreeParam;
    public FrameParamData SizeFourParam => _sizeFourParam;
    public FrameParamData SizeFiveParam => _sizeFiveParam;
    public FrameParamData SizeSixParam => _sizeSixParam;
    #endregion
}
