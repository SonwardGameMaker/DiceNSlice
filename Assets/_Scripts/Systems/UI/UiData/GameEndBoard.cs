using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameEndBoard : MonoBehaviour
{
    [SerializeField] private TMP_Text _victoryText;
    [SerializeField] private TMP_Text _defeatText;

    public void Show(bool isVictory)
    {
        gameObject.SetActive(true);

        _victoryText.gameObject.SetActive(isVictory);
        _defeatText.gameObject.SetActive(!isVictory);
    }

    public void SetActive(bool isActive)
        => gameObject.SetActive(isActive);
}
