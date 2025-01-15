using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterFrameParamsSingleton : MonoBehaviour
{
    #region fields
    private static CharacterFrameParamsSingleton _instance;

    private Dictionary<CharacterSize, FrameParamData> _charFrameParams;

    [Header("Params")]
    [SerializeField] private ParamsSO _initSO;
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

    public void Setup()
        => InitDictionary();

    private CharacterFrameParamsSingleton() { } // idk if this is needed, becouse it's Mono class

    private void InitDictionary()
    {
        _charFrameParams = new Dictionary<CharacterSize, FrameParamData>();
        _charFrameParams.Add(CharacterSize.One, _initSO.SizeOneParam);
        _charFrameParams.Add(CharacterSize.Two, _initSO.SizeTwoParam);
        _charFrameParams.Add(CharacterSize.Three, _initSO.SizeThreeParam);
        _charFrameParams.Add(CharacterSize.Four, _initSO.SizeFourParam);
        _charFrameParams.Add(CharacterSize.Five, _initSO.SizeFiveParam);
        _charFrameParams.Add(CharacterSize.Six, _initSO.SizeSixParam);
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

                    DontDestroyOnLoad(characterParamSingleton);
                }
            }

            return _instance;
        }
    }
    #endregion

    #region external interactions
    public FrameParamData GetFrameSize(CharacterSize size)
        => _charFrameParams.GetValueOrDefault(size);
    #endregion
}
