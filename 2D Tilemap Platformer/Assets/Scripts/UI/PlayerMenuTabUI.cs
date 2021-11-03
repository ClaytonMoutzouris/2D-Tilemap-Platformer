using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMenuTabUI : MenuTabUI
{
    public int playerIndex;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetPlayerIndex(int index)
    {
        playerIndex = index;
    }

    public override void SetAnchor()
    {
        if (GamepadInputManager.instance == null || GamepadInputManager.instance.gamepadInputs == null)
        {
            return;
        }

        if (GamepadInputManager.instance.gamepadInputs[playerIndex] != null)
        {
            //GamepadInputManager.instance.gamepadInputs[playerIndex].GetComponent<EventSystem>().SetSelectedGameObject(anchorObject);
            StartCoroutine(UIUtilities.SelectAnchorObject(GamepadInputManager.instance.gamepadInputs[playerIndex].GetComponent<EventSystem>(), anchorObject));
        }
    }

}
