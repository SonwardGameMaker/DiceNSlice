using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDiceManager
{
    public event Action<Dice> OnDiceChanged;

    public void Setup(ICombatManager combatManager);

    public DiceController GetControllerByCharacter(Character character);
}
