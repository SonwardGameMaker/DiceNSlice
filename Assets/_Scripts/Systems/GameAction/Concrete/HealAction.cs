using System;
using System.Collections.Generic;

public class HealAction : GameAction, IChooseTargetAction
{
    public override event Action OnActionUsed;

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

        OnActionUsed?.Invoke();
    }
}
