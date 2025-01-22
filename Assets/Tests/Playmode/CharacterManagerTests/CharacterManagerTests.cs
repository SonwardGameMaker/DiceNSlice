using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class CharacterManagerTests
{
    #region fields
    private CharacterManager _characterManager;
    private Transform _heroContainer;
    private Transform _enemyContainer;
    #endregion

    #region init
    [SetUp]
    public void SetUpBase()
    {
        GameObject characterManagerGo = new GameObject($"{nameof(CharacterManager)}");
        _characterManager = characterManagerGo.AddComponent<CharacterManager>();

        _heroContainer = new GameObject("HeroContainer").transform;
        _enemyContainer = new GameObject("EnemyContainer").transform;
    }

    public void CleanSetUp()
    {
        SetUpBase();

        _characterManager.TestSetup(_heroContainer, _enemyContainer);
    }

    [TearDown]
    public void TearDown()
    {
        GameObject.Destroy(_characterManager.gameObject);
        GameObject.Destroy(_heroContainer.gameObject);
        GameObject.Destroy(_enemyContainer.gameObject);
    }
    #endregion

    #region CreateHero tests
    [UnityTest]
    public IEnumerator HeroCreate_IfSortProperly()
    {
        // Arrange
        CleanSetUp();

        List<HeroTestsSO> heroes = new List<HeroTestsSO>()
        {
            ScriptableObject.CreateInstance<HeroTestsSO>(),
            ScriptableObject.CreateInstance<HeroTestsSO>(),
            ScriptableObject.CreateInstance<HeroTestsSO>(),
            ScriptableObject.CreateInstance<HeroTestsSO>(),
            ScriptableObject.CreateInstance<HeroTestsSO>()
        };

        SetHeroParams(heroes[0], "OrangeHero", 5, 5, HeroClass.Orange);
        SetHeroParams(heroes[1], "GreyHero", 6, 6, HeroClass.Gray);
        SetHeroParams(heroes[2], "BlueHero", 3, 3, HeroClass.Blue);
        SetHeroParams(heroes[3], "YellowHero", 5, 5, HeroClass.Yellow);
        SetHeroParams(heroes[4], "RedHero", 4, 4, HeroClass.Red);

        // Act
        foreach(HeroSO hero in heroes)
            _characterManager.AddHero(hero);

        heroes = heroes.OrderBy(h => (int)h.HeroClass).ToList();

        // Assert
        Assert.AreEqual(heroes.Count, _characterManager.Heroes.Count, "Кількість героїв у списку повинна дорівнювати доданій кількості.");

        for (int i = 0; i < _characterManager.Heroes.Count; i++)
        {
            Assert.AreEqual(heroes[i].HeroClass, _characterManager.Heroes[i].HeroClass,
                $"Герой на позиції {i + 1} повинен мати HeroClass {heroes[i].HeroClass}.");
        }

        yield return null;
    }
    #endregion

    #region internal interactions
    private void SetHeroParams(HeroTestsSO so, string name, int maxHealth, int currentHealth, HeroClass heroClass)
    {
        so.SetName(name);
        so.SetMaxHealth(maxHealth);
        so.SetCurrentHealth(currentHealth);
        so.SetHeroClass(heroClass);
    }
    #endregion
}
