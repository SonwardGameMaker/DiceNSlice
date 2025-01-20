using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class GameManagerBase : MonoBehaviour
{
    #region init
    // Character Manager
    protected abstract void CharacterManagerSubscribe();
    protected abstract void CharacterManagerUnsubscribe();

    // Combat Manager
    protected abstract void CombatManagerSubscription();
    protected abstract void CombatManagerUnsubscription();

    // UI Manager
    protected abstract void UiManagerSubscription();
    protected abstract void UimanagerUnsubscription();

    // Input Manager
    protected abstract void InputManagerSubscription();
    protected abstract void InputManagerUnsubscription();
    #endregion
}
