using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    private List<EnemyPassive> _passieves;

    #region init
    public override void Setup(CharacterSO so)
    {
        if (so is HeroSO)
            Setup(so as HeroSO);
        else
            base.Setup(so);
    }

    public void Setup(EnemySO so)
    {
        base.Setup(so);
    }
    #endregion

    public List<EnemyPassive> Passives { get => _passieves; }
}
