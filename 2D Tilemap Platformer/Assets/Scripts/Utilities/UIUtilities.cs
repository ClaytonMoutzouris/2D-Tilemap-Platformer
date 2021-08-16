using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class UIUtilities
{
    public static IEnumerator SelectAnchorObject(EventSystem eventSystem, GameObject anchor)
    {
        eventSystem.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        eventSystem.SetSelectedGameObject(anchor);

    }
}
