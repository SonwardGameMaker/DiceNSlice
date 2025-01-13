using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    #region fields
    [Header("Prefabs")]
    [SerializeField] private GameObject _heroFramePrefab;
    [SerializeField] private GameObject _enemyFramePrefab;

    [Header("UI Components")]
    [SerializeField] private VerticalLayoutGroup _heroes;
    [SerializeField] private VerticalLayoutGroup _enemies;
    [SerializeField] private ButtonUiController _buttons;

    [Header("Other")]
    [SerializeField] int _deltaMoveCoef;

    private bool _isSet = false;
    private float deltaMove;
    #endregion

    #region init
    public void Setup(List<Hero> heroes, List<Enemy> enemies)
    {
        if (_isSet) return;

        deltaMove = _heroFramePrefab.GetComponent<RectTransform>().sizeDelta.x / _deltaMoveCoef;
        Debug.Log($"Delta Move: {deltaMove}");
        SetHeroes(heroes);
        SetEnemies(enemies);
    }
    #endregion

    #region external interactions
    public void SetHeroes(List<Hero> heroes)
    {
        foreach (var hero in heroes)
            AddCharacter(hero);
    }

    public void SetEnemies(List<Enemy> enemies)
    {
        foreach(var enemy in enemies)
            AddCharacter(enemy);
    }

    public void AddCharacter(Character character)
    {
        GameObject prefab;
        VerticalLayoutGroup characterGroup;
        if (character is Hero)
        {
            characterGroup = _heroes;
            prefab = _heroFramePrefab;
        }
        else
        {
            characterGroup = _enemies;
            prefab = _enemyFramePrefab;
        }

        GameObject go = Instantiate(prefab, Vector3.zero, Quaternion.identity);
        go.transform.SetParent(characterGroup.transform, false);
        go.GetComponentInChildren<CharacterFrame>().Setup(character);
    }

    public bool RemoveCharacter(Character character)
    {
        var frame = GetCharacterFrame(character);
        if (frame != null)
        {
            Destroy(frame);
            return true;
        }

        return false;
    }

    public void MoveCharacterForvard(Character character)
    {
        var frame = GetCharacterFrame(character);
        if (frame != null)
        {
            frame.MoveFrame(deltaMove);
        }
    }

    public void MoveCharacterBack(Character character)
    {
        var frame = GetCharacterFrame(character);
        if (frame != null)
        {
            frame.ResetPosition();
        }
    }

    public bool IsUiElementSelected(Vector3 position)
    {
        List<RaycastResult> raycastResults = GetRaycastResults(position);

        foreach (var raycastResult in raycastResults)
        {
            if (raycastResult.gameObject.layer == LayerMask.GetMask("UI"))
            {
                return true;
            }
        }

        return false;
    }

    public Character IsCharacterSelected(Vector3 position)
    {
        List<RaycastResult> raycastResults = GetRaycastResults(position);
        Debug.Log($"raycast result count: {raycastResults.Count}");

        foreach (var raycastResult in raycastResults)
        {
            Debug.Log("Raycast result" + raycastResult);
            CharacterFrame characterFrame = raycastResult.gameObject.GetComponentInChildren<CharacterFrame>();
            if (characterFrame != null)
            {
                return characterFrame.Character;
            }
        }

        return null;
    }
    #endregion

    #region internal operations
    private CharacterFrame GetCharacterFrame(Character character)
    {
        VerticalLayoutGroup characterGroup;
        if (character is Hero)
            characterGroup = _heroes;
        else
            characterGroup = _enemies;


        for (int i = 0; i < characterGroup.transform.childCount; i++)
        {
            var tempCharacter = characterGroup.transform.GetChild(i).GetComponent<CharacterFrame>();
            if (tempCharacter.Character == character)
            {
                return tempCharacter;
            }
        }

        return null;
    }

    private List<RaycastResult> GetRaycastResults(Vector3 position)
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = position
        };

        Debug.Log($"Position: {position.x}, {position.y}, {position.z}");

        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, raycastResults);

        return raycastResults;
    }
    #endregion
}
