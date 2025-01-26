using System.Collections.Generic;

public class ShieldAllAction : GameAction, IAllTargetsAction
{
    public ShieldAllAction(int basePips, bool usingPips = true) : base(basePips, usingPips)
    {
        _name = "ShieldAll";
        _description = $"Give {_basePips} shields to all allies";
    }

    public override List<Character> GetValidTargets(List<Character> allies, List<Character> enemies)
        => allies;

    public void UseOn(List<Character> characters, int pips)
    {
        if (characters == null || characters.Count == 0) return;

        foreach (Character character in characters)
            character.ChangeShields(pips);
    }

    public void UndoUsing(List<Character> characters, int pips)
    {
        if (characters == null || characters.Count == 0) return;

        foreach (Character character in characters)
            character.ChangeShields(-pips);
    }
}
