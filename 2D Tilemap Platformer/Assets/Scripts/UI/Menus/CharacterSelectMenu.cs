using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//This houses all of the players
public class CharacterSelectMenu : MonoBehaviour
{
    public static CharacterSelectMenu instance;
    public CharacterSelectScreen[] characterSelectScreens = new CharacterSelectScreen[4];
    // Start is called before the first frame update
    void Start()
    {
        instance = this;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnEnable()
    {
        if (GamepadInputManager.instance == null || GamepadInputManager.instance.gamepadInputs == null)
        {
            return;
        }

        foreach (NewGamepadInput input in GamepadInputManager.instance.gamepadInputs)
        {
            if (input == null)
            {
                continue;
            }

            characterSelectScreens[input.input.playerIndex].gameObject.SetActive(true);

            //Add a character select screen
            //input.GetComponent<EventSystem>().SetSelectedGameObject(anchorObject);
        }
    }

    public void NewPlayerJoined(int index)
    {
        characterSelectScreens[index].gameObject.SetActive(true);

    }

    public void DropPlayer(int index)
    {

        GamepadInputManager.instance.DropPlayer(index);
        characterSelectScreens[index].gameObject.SetActive(false);

    }

    public bool AllPlayersReady()
    {

        foreach(CharacterSelectScreen selectScreen in characterSelectScreens)
        {
            if(GamepadInputManager.instance.gamepadInputs[selectScreen.playerIndex] == null)
            {
                continue;
            }

            if(!selectScreen.playerReady)
            {
                return false;
            }
        }

        return true;
    }

    //We need something that listens for a player joined event or something here
}
