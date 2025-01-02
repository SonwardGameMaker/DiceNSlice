using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TempGameManager : MonoBehaviour
{
    [SerializeField] private CombatManager _combatManager;

    [Header("Heroes")]
    [SerializeField] private HeroSO _hero1;
    [SerializeField] private HeroSO _hero2;
    [SerializeField] private HeroSO _hero3;
    [SerializeField] private HeroSO _hero4;
    [SerializeField] private HeroSO _hero5;

    [Header("Enemies")]
    [SerializeField] private List<EnemySO> _enemies;

    #region init
    void Start()
    {
        _combatManager.Setup(new List<HeroSO> { _hero1, _hero2, _hero3, _hero4, _hero5 }, _enemies);
    }
    #endregion

   
}
