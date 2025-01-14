using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterFrame : MonoBehaviour
{
    #region fields
    [SerializeField] private Character _character;

    [SerializeField] private TMP_Text _charName;
    [SerializeField] private Image _portraitImage;
    [SerializeField] private HealthPointsLabel _healthPointsLabel;

    RectTransform _rectTransform;
    private Vector2 _defaultPosition;
    private bool _isMoved;
    #endregion

    #region init
    public void Setup()
    {
        _rectTransform = GetComponent<RectTransform>();
        _defaultPosition = Vector2.zero;
        SetupFrameSize();

        _charName.text = _character.Name;
        _portraitImage.sprite = _character.Portrait;
        _healthPointsLabel.Setup(_character.MaxHealth, _character.CurrentHealth);
    }

    public void Setup(Character character)
    {
        _character = character;
        Setup();
    }

    private void SetupFrameSize()
    {
        RectTransform parentRectTransform = GetComponentInParent<RectTransform>();
        FrameParamData frameParamData = CharacterFrameParamsSingleton.GetFrameSize(_character.CharacterSize);

        parentRectTransform.sizeDelta = frameParamData.MainFrameParams;
        _portraitImage.GetComponent<RectTransform>().sizeDelta = frameParamData.PortraitFrameParams;

        //TODO
    }
    #endregion

    #region properties
    public Character Character { get => _character; }
    #endregion

    #region external interactions
    public void MoveFrame(float delta)
    {
        if (_isMoved)
        {
            Debug.LogError("Character cannot move becouse he is already moved");
            return;
        }

        _rectTransform.anchoredPosition = _rectTransform.anchoredPosition + new Vector2(delta, 0f);
        _isMoved = true;
    }

    public void ResetPosition()
    {
        if (!_isMoved) return;

        _rectTransform.anchoredPosition = _defaultPosition;
        _isMoved = false;
    }
    #endregion
}
