using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonUiController : MonoBehaviour
{
    [SerializeField] private RerollButton _rerollButton;
    [SerializeField] private UndoRollButton _undoRollButton;
    [SerializeField] private NextButton _nextButton;

    public RerollButton RerollButton { get => _rerollButton; }
    public UndoRollButton UndoRollButton { get => _undoRollButton; }
    public NextButton NextButton { get => _nextButton; }
}
