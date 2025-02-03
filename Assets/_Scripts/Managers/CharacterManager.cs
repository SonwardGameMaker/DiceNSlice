using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.TextCore.Text;
using static UnityEngine.EventSystems.EventTrigger;

public class CharacterManager : MonoBehaviour
{
    #region fields
    [Header("Containers")]
    [SerializeField] private Transform _heroContainer;
    [SerializeField] private Transform _enemyContainer;

    private List<Hero> _heroes;
    private List<Enemy> _enemies;
    #endregion

    #region events
    public event Action<Character> OnCharacterCreated;
    public event Action<Character> OnCharacterDeleted;
    public event Action<Character> OnCharacterChanged;
    #endregion

    #region init
    // Setup for testing classes
    public void TestSetup(Transform heroContainer, Transform enemyContainer)
    {
        _heroContainer = heroContainer;
        _enemyContainer = enemyContainer;

        Setup();
    }

    public void TestSetup(Transform heroContainer, Transform enemyContainer, List<HeroSO> heroes, List<EnemySO> enemies)
    {
        _heroContainer = heroContainer;
        _enemyContainer = enemyContainer;

        Setup(heroes, enemies);
    }

    public void TestSetup(Transform heroContainer, Transform enemyContainer, List<Hero> heroes, List<Enemy> enemies)
    {
        _heroContainer = heroContainer;
        _enemyContainer = enemyContainer;

        Setup(heroes, enemies);
    }

    // Standart setup
    public void Setup()
    {
        _heroes = new List<Hero>();
        _enemies = new List<Enemy>();
    }

    public void Setup(List<HeroSO> heroes, List<EnemySO> enemies)
        => Setup(CreateHeroes(heroes), CreateEnemies(enemies));

    private void Setup(List<Hero> heroes, List<Enemy> enemies)
    {
        if (heroes == null)
            heroes = new List<Hero>();
        else
            _heroes = heroes;

        if (enemies == null)
            enemies = new List<Enemy>();
        else
            _enemies = enemies;
    }
    #endregion

    #region properties
    public List<Hero> Heroes => _heroes;
    public List<Enemy> Enemies => _enemies;
    #endregion

    #region exteral interactions heroes
    public void AddHeroes(List<HeroSO> sOs)
    {
        if (_heroes.Count + sOs.Count > MaxHeroCount)
            return;

        List<Hero> heroes = CreateHeroes(sOs);

        _heroes.AddRange(heroes);
        _heroes = _heroes.OrderBy(h => (int)h.HeroClass).ToList();
    }

    public void AddHero(HeroSO so)
    {
        if (_heroes.Count >= MaxHeroCount) 
            return;

        _heroes.Add(CreateHero(so));
        _heroes = _heroes.OrderBy(h => (int)h.HeroClass).ToList();
    }

    public void DeleteHero(Hero hero)
    {
        _heroes.Remove(hero);
        _deadHeroes.Remove(hero);
        DeleteCharacter(hero, _heroContainer);
    }

    public void ResetHeroesShields()
        => ResetShields(_heroes);
    #endregion

    #region exteral interactions enemies
    public void AddEnemies(List<EnemySO> enemies)
    {
        (List<Enemy>, List<Enemy>) result = SplitIntoReinforcements(enemies.Select(h => CreateEnemy(h)).ToList());
        _enemies.AddRange(result.Item1);
        _reinforcementEnemies.AddRange(result.Item2);
        RefreshEnemiesData();
    }

    public void AddEnemy(EnemySO so)
        => _enemies.Add((Enemy)CreateCharacter<Enemy>(so, _enemyContainer, "_Enemy"));

    public void DeleteEnemy(Enemy enemy)
    {
        _enemies.Remove(enemy);
        DeleteCharacter(enemy, _enemyContainer);
    }

    public void ResetEnemiesShields()
        => ResetShields(_enemies);
    #endregion

    #region internal operations
    private Character CreateCharacter<T>(CharacterSO so, Transform container, string nameSuffix) where T : Character
    {
        GameObject character = new GameObject(so.Name + nameSuffix);
        character.transform.SetParent(container.transform);

        T result = character.AddComponent<T>();
        Dice dice = character.AddComponent<Dice>();
        result.Setup(so);
        dice.Setup(result, so.GetDiceSides());

        OnCharacterCreated?.Invoke(result);
        result.OnCharacterChanged += OnCharacterChangedHandler;

        return result;
    }

    private bool DeleteCharacters(List<Character> characters, Transform container)
    {
        if (container == null || characters == null || characters.Count == 0)
            return false;

        bool result = false;

        foreach (Character character in characters)
            if (DeleteCharacter(character, container))
                result = true;

        return result;
    }

    private bool DeleteCharacter(Character character, Transform container)
    {
        if (container == null || character == null)
            return false;

        foreach (Transform child in container)
        {
            Character childCharacter = child.GetComponent<Character>();
            if (childCharacter != null && childCharacter == character)
            {
                character.OnCharacterChanged -= OnCharacterChangedHandler;
                OnCharacterDeleted?.Invoke(character);
                Destroy(child.gameObject);
                return true;
            }
        }

        return false;
    }

    private void ResetShields<T>(List<T> characters) where T : Character
    {
        foreach (T character in characters)
            character.ResetShields();
    }
    #endregion

    #region internal operations heroes
    private List<Hero> CreateHeroes(List<HeroSO> heroes)
    {
        if (heroes == null) return null;

        List<Hero> result = heroes.Select(h => CreateHero(h)).ToList();
        result = result.OrderBy(h => (int)h.HeroClass).ToList();
        return result;
    }

    private Hero CreateHero(HeroSO so)
        => (Hero)CreateCharacter<Hero>(so, _heroContainer, "_Hero");
    #endregion

    #region internal operations enemies
    public List<Enemy> CreateEnemies(List<EnemySO> enemies)
    {
        if (enemies == null) return null;

        return enemies.Select(h => CreateEnemy(h)).ToList();
    }

    public Enemy CreateEnemy(EnemySO so)
        => (Enemy)CreateCharacter<Enemy>(so, _enemyContainer, "_Enemy");
    #endregion

    #region event hanlers
    private void OnCharacterChangedHandler(Character character)
    {
        CheckEnemiesLines();

        if (character.CurrentHealth > 0)
            OnCharacterChanged?.Invoke(character);
        else
        {
            if (character is Hero)
                HeroDies(character as Hero);
            else
                EnemyDies(character as Enemy);
        }
    }
    #endregion
}
