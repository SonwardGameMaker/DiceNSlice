using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFlowMagager
{
    #region fields
    private GameSystemsManager _gameSystemsManager;

    private int _levelCount;
    #endregion

    #region init
    public GameFlowMagager(GameSystemsManager gameSystemsManager)
    {
        _gameSystemsManager = gameSystemsManager;

        _levelCount = 1;
        
        _gameSystemsManager.CombatManager.OnCombatEnded += OnCombatEndedHandler;
    }

    ~GameFlowMagager()
    {
        _gameSystemsManager.CombatManager.OnCombatEnded -= OnCombatEndedHandler;
    }
    #endregion

    public void StartGame()
    {
        _gameSystemsManager.CombatManager.StartCombat();
    }

    #region event handlers
    private void OnCombatEndedHandler(bool isVictory)
    {
        // TODO
    }
    #endregion
}
