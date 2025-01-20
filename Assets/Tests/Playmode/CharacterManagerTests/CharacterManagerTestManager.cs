using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class CharacterManagerTestManager
{
    [UnityTest]
    public IEnumerator CharacterManagerTestManagerWithEnumeratorPasses()
    {
        (CharacterManager, Transform, Transform) init = Init_ClearContainers();

        yield return null;
    }

    #region interanl operations
    /// <summary>
    /// This method can create CharacterManager and setup it with clear hero and enemy containers
    /// </summary>
    /// <returns> First Transform is for heroes, second - for eneies </returns>
    private (CharacterManager, Transform, Transform) Init_ClearContainers()
    {
        GameObject characterManagerGo = new GameObject($"{nameof(CharacterManager)}");
        CharacterManager characterManager = characterManagerGo.AddComponent<CharacterManager>();

        Transform heroContainer = new GameObject("HeroContainer").transform;
        Transform enemyContainer = new GameObject("EnemyContainer").transform;

        characterManager.TestSetup(heroContainer, enemyContainer);

        return new(characterManager, heroContainer, enemyContainer);
    }
    #endregion
}
