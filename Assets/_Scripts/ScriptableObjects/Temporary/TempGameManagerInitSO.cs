using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObects/Temp/GameManagerInitSO")]
public class TempGameManagerInitSO : ScriptableObject
{
    [SerializeField] private List<HeroSO> _heroes;
    [SerializeField] private List<EnemySO> _enemies;

    public List<HeroSO> Heroes { get => _heroes; }
    public List<EnemySO> Enemies { get => _enemies;  }
}
