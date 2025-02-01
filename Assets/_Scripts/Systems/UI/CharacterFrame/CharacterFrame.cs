using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class CharacterFrame : MonoBehaviour
{
    #region fields
    [SerializeField] private TMP_Text _charName;
    [SerializeField] private UnityEngine.UI.Image _portraitImage;
    [SerializeField] private HealthPointsLabel _healthPointsLabel;
    [SerializeField] private UnityEngine.UI.Image _shieldsImage;
    [SerializeField] private TMP_Text _shieldCount;

    [Header("Dice cell image")]
    [SerializeField] private UnityEngine.UI.Image _diceCellImage;
    [SerializeField] private UnityEngine.UI.Image _rolledDiceSide;
    [SerializeField] private TMP_Text _pipsCount;
    [SerializeField] private UnityEngine.UI.Image _usageIndicator;

    private Character _character;
    private Dice _dice;

    private RectTransform _rectTransform;
    private Vector2 _defaultPosition;
    private bool _isMoved;
    #endregion

    #region init
    public void Setup()
    {
        _rectTransform = GetComponent<RectTransform>();
        _defaultPosition = Vector2.zero;
        SetupFrameSize();

        UpdateData();
        UpdateDiceData();

        // TODO: Make scaling for shield icon
        void SetupFrameSize()
        {
            if (_character == null) throw new NullReferenceException("Cannot setup character frame without character");

            RectTransform parentRectTransform = transform.parent.GetComponent<RectTransform>();
            RectTransform portraitRectTransform = _portraitImage.transform.parent.GetComponent<RectTransform>();
            RectTransform diceCellRectTransform = _diceCellImage.GetComponent<RectTransform>();
            FrameParamData frameParamData = CharacterFrameParamsSingleton.Instance.GetFrameSize(_character.CharacterSize);

            parentRectTransform.sizeDelta = new Vector2(parentRectTransform.sizeDelta.x, frameParamData.MainFrameHeight);
            portraitRectTransform.sizeDelta = new Vector2(frameParamData.SquareComponentsSide, frameParamData.SquareComponentsSide);
            diceCellRectTransform.sizeDelta = new Vector2(frameParamData.SquareComponentsSide, frameParamData.SquareComponentsSide);

            int coef = _character is Hero ? 1 : -1;
            portraitRectTransform.anchoredPosition = new Vector2(coef * frameParamData.SquareComponentsPadding, 0f);
            diceCellRectTransform.anchoredPosition = new Vector2(-coef * frameParamData.SquareComponentsPadding, 0f);
        }
    }

    public void Setup(Character character)
    {
        _character = character;
        _dice = character.Dice;
        Setup();
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
            Debug.LogError("Character cannot move because he is already moved");
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

    public void UpdateData()
    {
        _charName.text = _character.Name;
        _portraitImage.sprite = _character.Portrait;
        _healthPointsLabel.Setup(_character.MaxHealth, _character.CurrentHealth);
        SetShields(_character.Shields);
    }

    public void UpdateDiceData()
    {
        if (_dice.RolledSide == null)
            return;

        _rolledDiceSide.sprite = _dice.RolledSide.Sprite;
        _pipsCount.text = _dice.RolledSide.CurrentPips.ToString();

        if (_dice.RolledSide.Enabled) _usageIndicator.gameObject.SetActive(false);
        else _usageIndicator.gameObject.SetActive(true);
    }

    public void SetRolledDice()
    {
        if (_character.Dice.RolledSide == null) 
            return;

        _rolledDiceSide.sprite = _character.Dice.RolledSide.Sprite;
        _pipsCount.text = _character.Dice.RolledSide.CurrentPips.ToString();

        // TODO: make it normal
    }

    public void SetActive(bool isActive)
        => transform.parent.gameObject.SetActive(isActive);
    #endregion

    #region internal operations
    private void SetShields(int amount)
    {
        if (amount > 0)
        {
            _shieldsImage.gameObject.SetActive(true);
            _shieldCount.text = amount.ToString();
        }
        else
            _shieldsImage.gameObject.SetActive(false);
    }
    #endregion
}
