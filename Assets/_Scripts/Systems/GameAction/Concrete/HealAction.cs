using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealAction : GameAction, ISingleTargetAction
{
    public HealAction(int basePips, bool usingPips = true) : base(basePips, usingPips)
    {
        _name = "Heal";
        _description = $"Heal {_basePips} health of an ally";
    }

    public override List<Character> GetValidTargets(List<Character> allies, List<Character> enemies)
        => allies;

    public void UseOn(Character character, int pips)
    {
        if (character == null) return;

        character.ChangeHp(pips);
    }

    public void UndoUsing(Character character, int pips)
    {
        if (character == null) return;

        character.ChangeHp(-pips);
    }
}
