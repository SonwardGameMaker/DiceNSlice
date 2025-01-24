using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RerollButton : MonoBehaviour
{
    #region fields
    [SerializeField] private TMP_Text _maxRerollNumbers;
    [SerializeField] private TMP_Text _currentRerollNumbers;
    #endregion

    #region init
    public void Setup(int maxRerolls, int currentRerolls)
    {
        SetMaxRerolls(maxRerolls);
        SetCurrentRerolls(currentRerolls);
    }
    #endregion

    #region external interactions
    public void SetMaxRerolls(int maxRerolls)
    {
        _maxRerollNumbers.text = maxRerolls.ToString();
    }

    public void SetCurrentRerolls(int  currentRerolls)
    {
        _currentRerollNumbers.text = currentRerolls.ToString();
    }
    #endregion
}
