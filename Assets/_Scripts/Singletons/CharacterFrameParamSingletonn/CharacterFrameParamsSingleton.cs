using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterFrameParamsSingleton : MonoBehaviour
{
    #region fields
    private static CharacterFrameParamsSingleton _instance;

    private static Dictionary<CharacterSize, FrameParamData> _charFrameParams;

    [Header("Params")]
    [SerializeField] private static FrameParamData _sizeOneParam;
    [SerializeField] private static FrameParamData _sizeTwoParam;
    [SerializeField] private static FrameParamData _sizeThreeParam;
    [SerializeField] private static FrameParamData _sizeFourParam;
    [SerializeField] private static FrameParamData _sizeFiveParam;
    [SerializeField] private static FrameParamData _sizeSixParam;
    #endregion

    #region init
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            InitDictionary();
            DontDestroyOnLoad(gameObject);
        }   
        else if (_instance != this)
            Destroy(gameObject);
    }

    private CharacterFrameParamsSingleton() { } // idk if this is needed, becouse it's Mono class

    private static void InitDictionary()
    {
        _charFrameParams = new Dictionary<CharacterSize, FrameParamData>();
        _charFrameParams.Add(CharacterSize.One, _sizeOneParam);
        _charFrameParams.Add(CharacterSize.Two, _sizeTwoParam);
        _charFrameParams.Add(CharacterSize.Three, _sizeThreeParam);
        _charFrameParams.Add(CharacterSize.Four, _sizeFourParam);
        _charFrameParams.Add(CharacterSize.Five, _sizeFiveParam);
        _charFrameParams.Add(CharacterSize.Six, _sizeSixParam);
    }
    #endregion

    #region properties
    public static CharacterFrameParamsSingleton Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindAnyObjectByType<CharacterFrameParamsSingleton>();
                
                if (_instance == null)
                {
                    GameObject characterParamSingleton = new GameObject(nameof(CharacterFrameParamsSingleton));
                    _instance = characterParamSingleton.AddComponent<CharacterFrameParamsSingleton>();
                    InitDictionary();

                    DontDestroyOnLoad(characterParamSingleton);
                }
            }

            return _instance;
        }
    }
    #endregion

    #region external interactions
    public static FrameParamData GetFrameSize(CharacterSize size)
        => _charFrameParams.GetValueOrDefault(size);
    #endregion
}
