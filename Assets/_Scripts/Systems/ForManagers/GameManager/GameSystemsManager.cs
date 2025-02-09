using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystemsManager
{
    private ICharacterManager _characterManager;
    private IDiceManager _diceManager;
    private ICombatManager _combatManager;
    private IUiManager _uiManager;
    private IInputManager _inputManager;

    public GameSystemsManager(
        ICharacterManager characterManager,
        IDiceManager diceManager,
        ICombatManager combatManager,
        IUiManager uiManager,
        IInputManager inputManager,
        TempGameManagerInitSO initSO)
    {
        _characterManager = characterManager;
        _diceManager = diceManager;
        _combatManager = combatManager;
        _uiManager = uiManager;
        _inputManager = inputManager;

        characterManager.Setup(initSO.Heroes, initSO.Enemies);
        combatManager.Setup(characterManager);
        combatManager.Setup(characterManager);
        uiManager.Setup(combatManager, diceManager, inputManager);
    }

    public ICharacterManager CharacterManager => _characterManager;
    public IDiceManager DiceManager => _diceManager;
    public ICombatManager CombatManager => _combatManager;
    public IUiManager UiManager => _uiManager;
    public IInputManager InputManager => _inputManager;
}
