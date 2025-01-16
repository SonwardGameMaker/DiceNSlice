using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    #region fields
    private PlayerInput _playerInput;
    private InputAction _interact;
    private InputAction _info;
    #endregion

    #region events
    public event Action<Vector3> OnInteractClicked;
    public event Action<Vector3> OnInfoClicked;

    // Buttons
    public event Action OnRerollClicked;
    public event Action OnCancelRerollClicked;
    public event Action OnDoneRerollingClicked;
    public event Action OnNextTurnClicked;
    #endregion

    #region external interactions
    public void RerollButtonPressed()
    {
        OnRerollClicked?.Invoke();
    }

    public void UndoRerollsButtonPressed()
    {
        OnCancelRerollClicked?.Invoke();
    }

    public void NextButtonPressed()
    {
        // TODO
        // This method need to work like this:
        // if (smt)
        //     OnDoneRerollingClicked?.Invoke();
        //  else
        //      OnNextTurnClicked?.Invoke();

        // Temp realization:
        OnNextTurnClicked?.Invoke();
    }
    #endregion

    #region internal interactions
    private void OnInteractTriggered(InputAction.CallbackContext context)
        => OnInteractClicked?.Invoke(Input.mousePosition); // this may not work correctly on tpuchscreen

    private void OnInfoTriggered(InputAction.CallbackContext context)
        => OnInfoClicked?.Invoke(Input.mousePosition); // this may not work correctly on tpuchscreen
    #endregion

    #region MonoBehaviour methods
    private void Awake()
    {
        _playerInput = new PlayerInput();

    }

    private void OnEnable()
    {
        _interact = _playerInput.UI.ActionInteract;
        _info = _playerInput.UI.InfoInteract;

        _playerInput.Enable();
        _interact.Enable();
        _info.Enable();

        _interact.performed += OnInteractTriggered;
        _info.performed += OnInfoTriggered;
    }

    private void OnDisable()
    {
        _interact.performed -= OnInteractTriggered;
        _info.performed -= OnInfoTriggered;

        _interact.Disable();
        _info.Disable();

        _playerInput.Disable();
    }
    #endregion
}
