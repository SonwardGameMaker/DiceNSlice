using System;
using System.Collections.Generic;

public class ShieldAction : GameAction, IChooseTargetAction
{
    public override event Action OnActionUsed;

    public ShieldAction(int basePips, bool usingPips = true) : base(basePips, usingPips)
    {
        _name = "Shield";
        _description = $"Give {_basePips} shields to an ally";
    }

    public override List<Character> GetValidTargets(List<Character> allies, List<Character> enemies)
        => allies;

    public void UseOn(Character character, int pips)
    {
        if (character == null) return;

        character.ChangeShields(pips);

        OnActionUsed?.Invoke();
    }
}
