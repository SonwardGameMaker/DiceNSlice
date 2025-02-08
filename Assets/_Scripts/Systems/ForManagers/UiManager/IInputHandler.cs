using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInputHandler
{
    public void Setup(IInputManager inputHandler, ICombatManager combatManager);
}
