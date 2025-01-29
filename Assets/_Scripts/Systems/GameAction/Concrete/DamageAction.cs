using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageAction : GameAction, IChooseTargetAction
{
    public DamageAction(int basePips, bool usingPips = true) : base(basePips, usingPips)
    {
        _name = "Damage";
        _description = $"Deal {_basePips} damage to an enemy";
    }

    public override List<Character> GetValidTargets(List<Character> allies, List<Character> enemies)
        => enemies;

    public void UseOn(Character character, int pips)
    {
        if (character == null) return;

        character.TakeDamage(pips);
    }
}
