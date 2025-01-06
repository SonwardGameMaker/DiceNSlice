using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    private bool _isSet = false;
    private float deltaMove;
    #endregion

    #region events
    public event Action<Character> OnCharacterClicked;
    #endregion

    #region init
    public void Setup(List<Hero> heroes, List<Enemy> enemies)
    {
        if (_isSet) return;

        deltaMove = _heroFramePrefab.GetComponent<RectTransform>().sizeDelta.x / 2f;
        Debug.Log($"Delta Move: {deltaMove}");
        SetHeroes(heroes);
        SetEnemies(enemies);
    }
    #endregion

    private void Update()
    {
        // TODO trigger OnCharacterClicked
    }

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
    #endregion

    #region internal operations
    private void MoveCharacterForvard(Character character)
    {
        var frame = GetCharacterFrame(character);
        if (frame != null)
        {
            frame.MoveFrame(deltaMove);
        }
    }

    private void MoveCharacterBack(Character character)
    {
        var frame = GetCharacterFrame(character);
        if (frame != null)
        {
            frame.ResetPosition();
        }
    }

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
    #endregion
}
