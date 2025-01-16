using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonUiController : MonoBehaviour
{
    #region fields
    [SerializeField] private RerollButton _rerollButton;
    [SerializeField] private UndoRollButton _undoRollButton;
    [SerializeField] private NextButton _nextButton;
    #endregion

    #region properties
    public RerollButton RerollButton => _rerollButton;
    public UndoRollButton UndoRollButton => _undoRollButton;
    public NextButton NextButton => _nextButton;
    #endregion
}
