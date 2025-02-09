using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region fields
    [SerializeField] private CharacterManager CharacterManager;
    [SerializeField] private DiceManager DiceManager;
    [SerializeField] private CombatManager CombatManager;
    [SerializeField] private UiManager UiManager;
    [SerializeField] private InputManager InputManager;

    [Header("Init")]
    [SerializeField] private TempGameManagerInitSO _initSO;

    private ICharacterManager _characterManager;
    private IDiceManager _diceManager;
    private ICombatManager _combatManager;
    private IUiManager _uiManager;
    private IInputManager _inputManager;
    #endregion

    #region init
    private void Start()
    {
        _characterManager = CharacterManager;
        _diceManager = DiceManager;
        _combatManager = CombatManager;
        _uiManager = UiManager;
        _inputManager = InputManager;

        _characterManager.Setup(_initSO.Heroes, _initSO.Enemies);
        _combatManager.Setup(_characterManager);
        _diceManager.Setup(_combatManager);
        _uiManager.Setup(_combatManager, _diceManager, _inputManager);

        // Event Subscription
        _combatManager.OnCombatEnded += OnCombatEndedHandler;

        _combatManager.StartCombat();
    }

    private void OnDestroy()
    {
        // Event Unsubscription
        _combatManager.OnCombatEnded -= OnCombatEndedHandler;
    }
    #endregion

    #region event handlers
    private void OnCombatEndedHandler(bool isVictory)
    {
        // TODO
    }
    #endregion
}
