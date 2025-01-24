using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextButton : MonoBehaviour
{
    #region fields
    [SerializeField] private Sprite _deadCountIndicatorSprite;
    [SerializeField] private HorizontalLayoutGroup _dyingHeroesIndicator;
    #endregion

    #region external interactions
    public void SetDyingCharacters(int value)
    {
        // TODO
    }
    #endregion
}
