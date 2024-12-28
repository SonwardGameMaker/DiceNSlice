using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    private List<EnemyPassive> _passieves;

    public List<EnemyPassive> Passives { get => _passieves; }
}
