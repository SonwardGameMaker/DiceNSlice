using System.Collections.Generic;

/// <summary>
/// Using for actions that are using for constant targets (topmost, all, etc.)
/// </summary>
public interface IConstTargetsAction
{
    public void UseOn(List<Character> characters, int pips);
}
