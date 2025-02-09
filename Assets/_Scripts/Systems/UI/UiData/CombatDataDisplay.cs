using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatDataDisplay : MonoBehaviour
{
    [SerializeField] private TurnsDisplay _turnsDisplay;
    [SerializeField] private GameEndBoard _gameEndBoard;

    public void Setup()
    {
        _turnsDisplay.Setup();
    }

    public TurnsDisplay TurnsDisplay => _turnsDisplay;
    public GameEndBoard GameEndBoard => _gameEndBoard;
}
