using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RerollButton : MonoBehaviour
{
    [SerializeField] private TMP_Text _maxRerollNumbers;
    [SerializeField] private TMP_Text _currentRerollNumbers;

    public void Setup(int maxRerolls, int currentRerolls)
    {
        SetMaxRerolls(maxRerolls);
        SetCurrentRerolls(currentRerolls);
    }

    public void SetMaxRerolls(int maxRerolls)
    {
        _maxRerollNumbers.text = maxRerolls.ToString();
    }

    public void SetCurrentRerolls(int  currentRerolls)
    {
        _currentRerollNumbers.text = currentRerolls.ToString();
    }
}
