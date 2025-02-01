using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.TextCore.Text;
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

    private List<CharacterFrame> _characterFrames;
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
        _characterFrames = new List<CharacterFrame>();
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
        foreach (var enemy in enemies)
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
        CharacterFrame frame = go.GetComponentInChildren<CharacterFrame>();
        frame.Setup(character);
        _characterFrames.Add(frame);
    }

    public void UpdateCharacter(Character character)
        => GetCharacterFrame(character).UpdateData();

    public void UpdateCharacterDice(Character character)
        => GetCharacterFrame(character).UpdateDiceData();

    public bool RemoveCharacter(Character character)
    {
        var frame = GetCharacterFrame(character);
        if (frame != null)
        {
            _characterFrames.Remove(frame);
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
        => GetCharacterFrame(character).SetActive(false);

    public void EnableCharacter(Character character)
        => GetCharacterFrame(character).SetActive(true);

    public CharacterFrame GetCharacterFrame(Character character)
    {
        if (character == null) return null;

        return _characterFrames.Find(cf => cf.Character == character);
    }

    /// <summary>
    /// Resets ChracterFrame local list with current Character Group childrens
    /// </summary>
    public void ResetCharacterFrameList()
    {
        List<CharacterFrame> result = new List<CharacterFrame>();

        for (int i = 0; i < _heroes.transform.childCount; i++)
            result.Add(_heroes.transform.GetChild(i).GetComponentInChildren<CharacterFrame>());

        for (int i = 0; i < _enemies.transform.childCount; i++)
            result.Add(_enemies.transform.GetChild(i).GetComponentInChildren<CharacterFrame>());

        _characterFrames = result;
    }
    #endregion

    #region static methods
    public static Vector3 ToWorldPosition(Vector3 position)
        => Camera.main.WorldToScreenPoint(position);
    #endregion
}
