using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObects/CharacterData/EnemySO")]
public class EnemySO : CharacterSO
{
    [SerializeField] private List<EnemyPassive> _enemyPassives;

    public List<EnemyPassive> EnemyPassives => _enemyPassives;
}
