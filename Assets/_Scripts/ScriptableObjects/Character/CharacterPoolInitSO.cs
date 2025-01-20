using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObects/Temp/Cmts/CharacterPoolInit")]
public class CharacterPoolInitSO : ScriptableObject
{
    [SerializeField] private List<Hero> _heroes;
    [SerializeField] private List<Enemy> _enemies;

    public List<Hero> Heroes => _heroes;
    public List<Enemy> Enemies => _enemies;
}
