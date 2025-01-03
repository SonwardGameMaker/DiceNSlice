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
    #endregion

    #region init
    public void Setup()
    {
        _charName.text = _character.Name;
        _portraitImage.sprite = _character.Portrait;
        _healthPointsLabel.Setup(_character.MaxHealth, _character.CurrentHealth);
    }

    public void Setup(Character character)
    {
        _character = character;
        Setup();
    }
    #endregion

    #region properties
    public Character Character { get => _character; }
    #endregion
}
