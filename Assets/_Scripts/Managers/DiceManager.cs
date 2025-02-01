using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DiceManager : MonoBehaviour
{
    #region fields
    [SerializeField] private Transform _heroDiceGroup;
    [SerializeField] private Transform _enemyDiceGroup;

    private List<DiceController> _heroDiceControllers;
    private List<DiceController> _enemyDiceControllers;
    #endregion

    #region events
    public event Action<Dice> OnDiceChanged;
    #endregion

    #region init
    public void Setup(List<Hero> heroes, List<Enemy> enemies)
        => Setup(heroes.Select(h => h.Dice).ToList(), enemies.Select(e => e.Dice).ToList());

    public void Setup(List<Dice> heroDices, List<Dice> enemyDices)
    {
        _heroDiceControllers = CreateHeroControllers(heroDices);
        _enemyDiceControllers = CreateEnemyControllers(enemyDices);
    }
    #endregion

    #region properties
    public List<DiceController> HeroDices => _heroDiceControllers;
    public List<DiceController> EnemyDices => _enemyDiceControllers;
    #endregion

    #region external interactions
    public DiceController GetControllerByCharacter(Character character)
    {
        if (character is Hero hero) return GetControllerByHero(hero);
        else if (character is Enemy enemy) return GetControllerByEnemy(enemy);
        else return null;
    }
    #endregion

    #region external interactions heroes
    public void AddHeroControllers(List<Dice> heroDices)
        => _heroDiceControllers.AddRange(CreateHeroControllers(heroDices));

    public void AddHeroController(Dice heroDice)
        => _heroDiceControllers.Add(CreateHeroController(heroDice));

    public DiceController GetControllerByHero(Hero hero)
        => _heroDiceControllers.FirstOrDefault(hdc => hdc.Dice.Owner == hero);

    public void EnableHeroDices()
        => EnableDices(_heroDiceControllers);

    public void RollHeroDices()
        => RollDices(_heroDiceControllers);
    #endregion

    #region external interactions enemies
    public void AddEnemyControllers(List<Dice> enemyDices)
        => CreateEnemyControllers(enemyDices);

    public void AddEnemyController(Dice enemyDice)
        => CreateEnemyController(enemyDice);

    public DiceController GetControllerByEnemy(Enemy enemy)
        => _enemyDiceControllers.FirstOrDefault(edc => edc.Dice.Owner == enemy);

    public void EnableEnemyDices()
        => EnableDices(_enemyDiceControllers);

    public void RollEnemyDices()
    {
        RollDices(_enemyDiceControllers);
        foreach(DiceController controller in _enemyDiceControllers)
            controller.LockTheDice();
    }
    #endregion

    #region internal operations
    private List<DiceController> CreateDiceControllers(List<Dice> dices, Transform characterGroup, string nameSuffix, bool enabled = true)
    {
        List<DiceController> result = new List<DiceController>();

        foreach (Dice dice in dices)
            result.Add(CreateDiceController(dice, characterGroup, nameSuffix, enabled));

        return result;
    }

    private DiceController CreateDiceController(Dice dice, Transform characterGroup, string nameSuffix, bool enabled = true)
    {
        GameObject go = new GameObject($"{nameSuffix}DiceController");
        go.transform.SetParent(characterGroup);
        DiceController result = go.AddComponent<DiceController>();
        result.Setup(dice, enabled);
        result.Dice.OnDiceChanged += OnDiceChangedHandler;

        return result;
    }

    private bool RemoveDiceController(DiceController diceController, List<DiceController> diceControllers)
    {
        if (diceController == null) return false;
        if (!diceControllers.Contains(diceController)) return false;

        diceController.Dice.OnDiceChanged -= OnDiceChangedHandler;
        diceControllers.Remove(diceController);
        Destroy(diceController);

        return true;
    }

    private void RollDices(List<DiceController> dices)
    {
        foreach(DiceController controller in dices)
            if (controller.Enabled)
                controller.RollTheDice();
    }

    private void EnableDices(List<DiceController> dices)
    {
        foreach (DiceController controller in dices)
            controller.EnableDices();
    }

    #endregion

    #region internal operations heroes
    private List<DiceController> CreateHeroControllers(List<Dice> dices, bool enabled = true)
        => CreateDiceControllers(dices, _heroDiceGroup, nameof(Hero), enabled);

    private DiceController CreateHeroController(Dice dice, bool enabled = true)
    {
        if (!(dice.Owner is Hero)) return null;

        return CreateDiceController(dice, _heroDiceGroup, nameof(Hero), enabled);
    }
    #endregion

    #region internal operations enemies
    private List<DiceController> CreateEnemyControllers(List<Dice> dices, bool enabled = true)
        => CreateDiceControllers(dices, _enemyDiceGroup, nameof(Enemy), enabled);

    private DiceController CreateEnemyController(Dice dice, bool enabled = true)
    {
        if (!(dice.Owner is Enemy)) return null;

        return CreateDiceController(dice, _enemyDiceGroup, nameof(Enemy), enabled);
    }
    #endregion

    #region event handlers
    private void OnDiceChangedHandler(Dice dice)
        => OnDiceChanged?.Invoke(dice);
    #endregion
}
