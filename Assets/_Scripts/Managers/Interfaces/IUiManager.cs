using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUiManager
{
    public void Setup(ICombatManager combatManager, IDiceManager diceManager, IInputManager inputManager);
}
