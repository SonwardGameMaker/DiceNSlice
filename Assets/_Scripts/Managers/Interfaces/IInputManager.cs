using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInputManager
{
    #region events
    public event Action<Vector3> OnInteractClicked;
    public event Action<Vector3> OnInfoClicked;

    // Buttons
    public event Action OnRerollClicked;
    public event Action OnCancelRerollClicked;
    public event Action OnDoneRerollingClicked;
    public event Action OnNextButtonClicked;
    #endregion
}
