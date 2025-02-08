using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputHandler : MonoBehaviour, IInputHandler
{
    #region fields
    private IInputManager _inputManager;
    private ICombatManager _combatManager;
    #endregion

    #region init
    public void Setup(IInputManager inputHandler, ICombatManager combatManager)
    {
        _inputManager = inputHandler;
        _combatManager = combatManager;

        InputEventsSubscription();

        void InputEventsSubscription()
        {
            _inputManager.OnInteractClicked += OnInteractionClickedHandler;
            _inputManager.OnNextButtonClicked += OnNextTurnClickedHandler;
        }
    }

    private void OnDestroy()
    {
        InputEventsUnubscription();

        void InputEventsUnubscription()
        {
            _inputManager.OnInteractClicked -= OnInteractionClickedHandler;
            _inputManager.OnNextButtonClicked -= OnNextTurnClickedHandler;
        }
    }
    #endregion

    #region event handlers
    private void OnInteractionClickedHandler(Vector3 position)
    {
        List<RaycastResult> raycastResults = UiDataGetter.GetRaycastResults(position);
        bool isOnUi = UiDataGetter.IsUiElementSelected(raycastResults);
        Character isOnCharacter = UiDataGetter.IsCharacterSelected(raycastResults);

        if (isOnCharacter != null)
        {
            _combatManager.SelectCharacter(isOnCharacter);
            return;
        }

        _combatManager.Cancel();

        if (isOnUi)
        {
            // TODO
        }
        else
        {
            Vector3 worldPosition = UiManager.ToWorldPosition(position);
            // TODO
        }
    }

    private void OnInfoClickedHandler(Vector3 position)
    {
        // TODO
        //Debug.Log($"Info Clicked on {position}");
    }

    private void OnRerollClickedHandler()
    {
        // TODO: Implement reroll clicked behavior
        //Debug.Log("Reroll button clicked.");
    }

    private void OnCancelRerollClickedHandler()
    {
        // TODO: Implement cancel reroll clicked behavior
        //Debug.Log("Cancel reroll button clicked.");
    }

    private void OnDoneRerollingClickedHandler()
    {
        // TODO: Implement done rerolling clicked behavior
        //Debug.Log("Done rerolling button clicked.");
    }

    private void OnNextTurnClickedHandler()
    {
        // TODO: Implement next turn clicked behavior
        Debug.Log("Next turn button clicked.");

        _combatManager.Next();
    }
    #endregion
}
