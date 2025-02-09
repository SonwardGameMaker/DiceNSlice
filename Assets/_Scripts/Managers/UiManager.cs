using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class UiManager : MonoBehaviour, IUiManager
{
    #region fields
    [Header("Prefabs")]
    [SerializeField] private GameObject _heroFramePrefab;
    [SerializeField] private GameObject _enemyFramePrefab;

    [Header("UI Components")]
    [SerializeField] private VerticalLayoutGroup _heroes;
    [SerializeField] private VerticalLayoutGroup _enemies;
    [SerializeField] private ButtonUiController _buttons;
    [SerializeField] private CombatDataDisplay _combatDataDisplay;

    [Header("Components")]
    [SerializeField] private InputHandler _inputHandler;

    [Header("Other")]
    [SerializeField] private int _deltaMoveCoef;

    private ICombatCharacterLists _characterLists;
    private ICombatManager _combatManager;
    private IDiceManager _diceManager;

    private List<CharacterFrame> _characterFrames;
    private float deltaMove;
    #endregion

    #region init
    public void Setup(ICombatManager combatManager, IDiceManager diceManager, IInputManager inputManager)
    {
        _diceManager = diceManager;
        _combatManager = combatManager;
        _characterLists = combatManager.CombatLists;
        _inputHandler.Setup(inputManager, combatManager);

        Init();
        SubscribeToCombatFlow();
        SubscribeToCharacters();
        SubscribeToDices();

        SetHeroes(_characterLists.PresentHeroes);
        SetEnemies(_characterLists.PresentEnemies);

        void SubscribeToCharacters()
        {
            _characterLists.OnCharacterEnterScene += OnCharacterEnterSceneHandler;
            _characterLists.OnCharacterLeaveScene += OnCharacterLeaveSceneHandler;
            _characterLists.OnCharacterChanged += OnCharacterChangedHandler;
            _characterLists.OnCharacterCreated += OnCharacterCreatedHandler;
            _characterLists.OnCharacterDeleted += OnCharacterDeletedHandler;
        }

        void SubscribeToDices()
        {
            _diceManager.OnDiceChanged += OnDiceChangedHandler;
        }

        void SubscribeToCombatFlow()
        {
            _combatManager.OnHeroActivated += OnHeroActivatedHandler;
            _combatManager.OnHeroDeactivated += OnHeroDeactivatedHandler;
            _combatManager.OnTurnEnded += OnTurnEndedHandler;

            _combatManager.OnCombatEnded += OnCombatEndedHandler;
        }
    }

    private void Init()
    {
        CharacterFrameParamsSingleton.Instance.Setup();

        _combatDataDisplay.Setup();

        deltaMove = _heroFramePrefab.GetComponent<RectTransform>().sizeDelta.x / _deltaMoveCoef;
        _characterFrames = new List<CharacterFrame>();
    }

    private void OnDestroy()
    {
        UnsubscribeToCombatFlow();
        UnsubscribeToDices();
        UnubscribeToCharacters();

        void UnubscribeToCharacters()
        {
            _characterLists.OnCharacterEnterScene -= OnCharacterEnterSceneHandler;
            _characterLists.OnCharacterLeaveScene -= OnCharacterLeaveSceneHandler;
            _characterLists.OnCharacterChanged -= OnCharacterChangedHandler;
            _characterLists.OnCharacterCreated -= OnCharacterCreatedHandler;
            _characterLists.OnCharacterDeleted -= OnCharacterDeletedHandler;
        }

        void UnsubscribeToDices()
        {
            _diceManager.OnDiceChanged -= OnDiceChangedHandler;
        }

        void UnsubscribeToCombatFlow()
        {
            _combatManager.OnHeroActivated -= OnHeroActivatedHandler;
            _combatManager.OnHeroDeactivated -= OnHeroDeactivatedHandler;
            _combatManager.OnTurnEnded -= OnTurnEndedHandler;
        }
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

    public void AddCharacter(Character character, bool isActive = true)
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
        frame.SetActive(isActive);
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

    #region event handlers
    // Character Lists Handlers
    private void OnCharacterEnterSceneHandler(Character character)
        => EnableCharacter(character);

    private void OnCharacterLeaveSceneHandler(Character character)
        => DisableCharacter(character);

    private void OnCharacterChangedHandler(Character character)
        => UpdateCharacter(character);

    // Characters
    private void OnHeroActivatedHandler(Hero hero)
        => MoveCharacterForvard(hero);

    private void OnHeroDeactivatedHandler(Hero hero)
        => MoveCharacterBack(hero);

    private void OnCharacterCreatedHandler(Character character, bool isActive)
        => AddCharacter(character, isActive);

    private void OnCharacterDeletedHandler(Character character)
        => RemoveCharacter(character);

    // Dices
    private void OnDiceChangedHandler(Dice dice)
        => UpdateCharacterDice(dice.Owner);

    // Combat flow
    private void OnTurnEndedHandler()
        => _combatDataDisplay.TurnsDisplay.SetCountText();

    private void OnCombatEndedHandler(bool isVictory)
        => _combatDataDisplay.GameEndBoard.Show(isVictory);
    #endregion
}
