using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Build;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    #region fields
    [SerializeField] private CharacterManager _characterManager;
    [SerializeField] private UiManager _uiManager;

    private List<GameAction> _heroesActionOrder;
    #endregion

    #region init
    public void Setup(List<HeroSO> heroes, List<EnemySO> enemies)
    {

    }
    #endregion

    
}
