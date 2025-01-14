using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterFrameParamsSingleton : MonoBehaviour
{
    #region fields
    private static CharacterFrameParamsSingleton _instance;

    [SerializeField] private static Dictionary<CharacterSize, FrameParamData> _charFrameParams;
    #endregion

    #region init
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }   
        else if (_instance != this)
            Destroy(gameObject);
    }

    private CharacterFrameParamsSingleton() { } // idk if this is needed, becouse it's Mono class
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
    public static FrameParamData GetFrameSize(CharacterSize size)
        => _charFrameParams.GetValueOrDefault(size);
    #endregion
}
