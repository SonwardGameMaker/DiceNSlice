using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    #region fields
    private List<EnemyPassive> _passives;
    #endregion

    #region init
    public override void Setup(CharacterSO so)
    {
        if (so is EnemySO)
            Setup(so as EnemySO);
        else
            base.Setup(so);
    }

    public void Setup(EnemySO so)
    {
        base.Setup(so);

        _passives = so.EnemyPassives;
    }
    #endregion

    #region properties
    public List<EnemyPassive> Passives => _passives;
    #endregion
}
