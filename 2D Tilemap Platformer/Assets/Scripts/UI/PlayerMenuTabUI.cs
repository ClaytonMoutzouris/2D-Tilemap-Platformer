using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMenuTabUI : MenuTabUI
{
    int playerIndex;
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
        Debug.Log("Player menu onEnable : " + playerIndex);
        if (GamepadInputManager.instance == null || GamepadInputManager.instance.gamepadInputs == null)
        {
            return;
        }

        Debug.Log("Player menu onEnable : " + playerIndex);

        Debug.Log("Player input - " + GamepadInputManager.instance.gamepadInputs[playerIndex]);
        if (GamepadInputManager.instance.gamepadInputs[playerIndex] != null)
        {
            //GamepadInputManager.instance.gamepadInputs[playerIndex].GetComponent<EventSystem>().SetSelectedGameObject(anchorObject);
            StartCoroutine(UIUtilities.SelectAnchorObject(GamepadInputManager.instance.gamepadInputs[playerIndex].GetComponent<EventSystem>(), anchorObject));
        }
    }

}
