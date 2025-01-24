using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TurnsDisplay : MonoBehaviour
{
    [SerializeField] private CombatManager _combatManager;
    [SerializeField] private TMP_Text _coutText;

    public void Setup()
    {
        SetCountText();
        _combatManager.OnTurnEnded += SetCountText;
    }

    private void SetCountText()
        => _coutText.text = _combatManager.StateMachine.TurnCount.ToString();
}
