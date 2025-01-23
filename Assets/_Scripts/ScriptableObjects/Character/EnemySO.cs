using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObects/CharacterData/EnemySO")]
public class EnemySO : CharacterSO
{
    #region fields
    [SerializeField] private List<EnemyPassive> _enemyPassives;
    [SerializeField] private bool _onBackline;
    #endregion

    #region properties
    public List<EnemyPassive> EnemyPassives => _enemyPassives;
    public bool OnBackline => _onBackline;
    #endregion
}
