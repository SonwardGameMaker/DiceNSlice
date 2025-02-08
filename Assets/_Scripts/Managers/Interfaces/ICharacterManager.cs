using System;
using System.Collections.Generic;

public interface ICharacterManager
{
    #region events
    public event Action<Character> OnCharacterCreated;
    public event Action<Character> OnCharacterDeleted;
    public event Action<Character> OnCharacterChanged;
    #endregion

    #region init
    public void Setup();
    // Temp
    public void Setup(List<HeroSO> heroes, List<EnemySO> enemies);
    #endregion

    #region properties
    public List<Hero> Heroes { get; }
    public List<Enemy> Enemies {  get; }
    #endregion
}
