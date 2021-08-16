using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMenuTabUI : MonoBehaviour
{
    public GameObject anchorObject;
    int playerIndex;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPlayerIndex(int index)
    {
        playerIndex = index;
    }

    public void OnEnable()
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

    public void GetAnchorObject()
    {

    }

    public void CloseTab()
    {
        gameObject.SetActive(false);
    }

    public void OpenTab()
    {
        gameObject.SetActive(true);
    }

    public void OpenTab(int playerIndex)
    {
        this.playerIndex = playerIndex;
        gameObject.SetActive(true);
    }

    public void DropPlayer()
    {
        GamepadInputManager.instance.DropPlayer(playerIndex);
        //GameManager.instance.RemovePlayerAtIndex(playerIndex);
    }
}
