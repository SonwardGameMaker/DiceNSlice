using System.Collections.Generic;

public interface IAllTargetsAction
{
    public void UseOn(List<Character> characters, int pips);
    public void UndoUsing(List<Character> characters, int pips);
}
