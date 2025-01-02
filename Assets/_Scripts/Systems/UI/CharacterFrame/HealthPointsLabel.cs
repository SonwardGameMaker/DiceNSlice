using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthPointsLabel : MonoBehaviour
{
    #region fields
    [SerializeField] TMP_Text _maxHealthText;
    [SerializeField] TMP_Text _currentHealthText;
    #endregion

    #region init
    public void Setup(int maxHealth, int currentHealth)
    {
        SetHealth(maxHealth, currentHealth);
    }
    #endregion

    #region external interactions
    public void SetHealth(int maxValue, int currentValue)
    {
        SetMaxHealth(maxValue);
        SetCurrentHealth(currentValue);
    }

    public void SetMaxHealth(int value) => _maxHealthText.text = value.ToString();

    public void SetCurrentHealth(int value) => _currentHealthText.text = value.ToString();
    #endregion
}
