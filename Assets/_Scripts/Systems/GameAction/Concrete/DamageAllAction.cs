using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageAllAction : GameAction, IAllTargetsAction
{
    public DamageAllAction(int basePips, bool usingPips = true) : base(basePips, usingPips)
    {
        _name = "DamageAll";
        _description = $"Deal {_basePips} damage to all enemies";
    }

    public override List<Character> GetValidTargets(List<Character> allies, List<Character> enemies)
        => enemies;

    public void UseOn(List<Character> characters, int pips)
    {
        if (characters == null || characters.Count == 0) return;

        foreach (Character character in characters)
            character.ChangeHp(-pips);
    }

    public void UndoUsing(List<Character> characters, int pips)
    {
        if (characters == null || characters.Count == 0) return;

        foreach (Character character in characters)
            character.ChangeHp(pips);
    }
}
