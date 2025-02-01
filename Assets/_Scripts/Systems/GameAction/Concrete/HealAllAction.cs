using System;
using System.Collections.Generic;

public class HealAllAction : GameAction, IConstTargetsAction
{
    public override event Action OnActionUsed;

    public HealAllAction(int basePips, bool usingPips = true) : base(basePips, usingPips)
    {
        _name = "HealAll";
        _description = $"Heal {_basePips} health of all allies";
    }

    public override List<Character> GetValidTargets(List<Character> allies, List<Character> enemies)
        => allies;

    public void UseOn(List<Character> characters, int pips)
    {
        if (characters == null || characters.Count == 0) return;

        foreach (Character character in characters)
            character.ChangeHp(pips);

        OnActionUsed?.Invoke();
    }
}
