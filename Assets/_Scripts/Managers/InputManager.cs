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
    #endregion

    #region monobehaviour methods
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

    #region internal interactions
    private void OnInteractTriggered(InputAction.CallbackContext context)
        => OnInteractClicked?.Invoke(Camera.main.ScreenToWorldPoint(Input.mousePosition));

    private void OnInfoTriggered(InputAction.CallbackContext context)
        => OnInfoClicked?.Invoke(Camera.main.ScreenToWorldPoint(Input.mousePosition));
    #endregion
}
