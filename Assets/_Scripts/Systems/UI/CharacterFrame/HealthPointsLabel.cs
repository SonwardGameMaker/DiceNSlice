using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthPointsLabel : MonoBehaviour
{
    [SerializeField] TMP_Text _maxHealthText;
    [SerializeField] TMP_Text _currentHealthText;

    [SerializeField] int _maxHealth;
    [SerializeField] int _currentHealth;
}
