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
    // debug
    [SerializeField] private TurnsDisplay _turnsDisplay;

    [Header("Other")]
    [SerializeField] int _deltaMoveCoef;

    private bool _isSet = false;
    private float deltaMove;
    #endregion

    #region init
    public void Setup()
    {
        if (_isSet) return;

        Init();
    }

    public void Setup(List<Hero> heroes, List<Enemy> enemies)
    {
        if (_isSet) return;

        Init();

        SetHeroes(heroes);
        SetEnemies(enemies);
    }

    private void Init()
    {
        CharacterFrameParamsSingleton.Instance.Setup();

        _turnsDisplay.Setup();

        deltaMove = _heroFramePrefab.GetComponent<RectTransform>().sizeDelta.x / _deltaMoveCoef;
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

    public void UpdateCharacter(Character character)
        => GetCharacterFrame(character).UpdateData();

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
        int coef = character is Hero ? 1 : -1;
        var frame = GetCharacterFrame(character);
        if (frame != null)
        {
            frame.MoveFrame(coef * deltaMove);
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

        return IsUiElementSelected(raycastResults);
    }
    public bool IsUiElementSelected(List<RaycastResult> raycastResults)
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

    public Character IsCharacterSelected(Vector3 position)
    {
        List<RaycastResult> raycastResults = GetRaycastResults(position);

        return IsCharacterSelected(raycastResults);
    }
    public Character IsCharacterSelected(List<RaycastResult> raycastResults)
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

    public List<RaycastResult> GetRaycastResults(Vector3 position)
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = position
        };

        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, raycastResults);

        return raycastResults;
    }

    public void DisableCharacter(Character character)
        => GetCharacterFrame(character).enabled = false;

    public void EnableCharacter(Character character)
        => GetCharacterFrame(character).enabled = true;
    #endregion

    #region static methods
    public static Vector3 ToWorldPosition(Vector3 position)
        => Camera.main.WorldToScreenPoint(position);
    #endregion

    #region internal operations
    private CharacterFrame GetCharacterFrame(Character character)
    {
        VerticalLayoutGroup characterGroup = character is Hero ? _heroes : _enemies;

        for (int i = 0; i < characterGroup.transform.childCount; i++)
        {
            var tempCharacter = characterGroup.transform.GetChild(i).GetComponentInChildren<CharacterFrame>();
            if (tempCharacter.Character == character)
            {
                return tempCharacter;
            }
        }

        return null;
    }
    #endregion
}
