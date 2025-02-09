using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class UiDataGetter
{
    public static bool IsUiElementSelected(Vector3 position)
    {
        List<RaycastResult> raycastResults = GetRaycastResults(position);

        return IsUiElementSelected(raycastResults);
    }
    public static bool IsUiElementSelected(List<RaycastResult> raycastResults)
    {
        foreach (var raycastResult in raycastResults)
        {
            if (raycastResult.gameObject.layer == LayerMask.GetMask("UI"))
            {
                return true;
            }
        }

        return false;
    }

    public static Character IsCharacterSelected(Vector3 position)
    {
        List<RaycastResult> raycastResults = GetRaycastResults(position);

        return IsCharacterSelected(raycastResults);
    }
    public static Character IsCharacterSelected(List<RaycastResult> raycastResults)
    {
        foreach (var raycastResult in raycastResults)
        {
            CharacterFrame characterFrame = raycastResult.gameObject.GetComponent<CharacterFrame>();
            if (characterFrame != null)
            {
                return characterFrame.Character;
            }
        }

        return null;
    }

    public static List<RaycastResult> GetRaycastResults(Vector3 position)
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = position
        };

        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, raycastResults);

        return raycastResults;
    }
}
