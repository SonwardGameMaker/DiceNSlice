using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region fields
    [SerializeField] private CharacterManager _characterManager;
    [SerializeField] private DiceManager _diceManager;
    [SerializeField] private CombatManager _combatManager;
    [SerializeField] private UiManager _uiManager;
    [SerializeField] private InputManager _inputManager;

    [Header("Init")]
    [SerializeField] private TempGameManagerInitSO _initSO;

    private GameSystemsManager _gameSystemsManager;
    private GameFlowMagager _gameFlowMagager; 
    #endregion

    #region init
    private void Start()
    {
        _gameSystemsManager = new GameSystemsManager(
            _characterManager,
            _diceManager,
            _combatManager,
            _uiManager,
            _inputManager,
            _initSO);

        _gameFlowMagager = new GameFlowMagager(_gameSystemsManager);
        
        _gameFlowMagager.StartGame();
    }
    #endregion
}
