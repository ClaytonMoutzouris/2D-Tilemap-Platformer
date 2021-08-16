using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class MainMenu : MonoBehaviour
{
    public static MainMenu instance;
    public List<MenuTabUI> menuTabs;
    // This should change based on the current menu tab that is active
    //(will need to be more elegant for menu tabs that seperate for each player)
    public int currentTabIndex = 0;
    //public MenuTabUI menuTab;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeTab(int index)
    {
        foreach(MenuTabUI tab in menuTabs)
        {
            tab.CloseTab();
        }

        menuTabs[index].OpenTab();
        currentTabIndex = index;
    }

    public GameObject GetCurrentAnchor()
    {
        return menuTabs[currentTabIndex].GetAnchorObject();
        //return currentAnchor;
    }

    public void NewPlayerJoined(PlayerInput newPlayer)
    {
        if(currentTabIndex == 2)
        {
            menuTabs[currentTabIndex].GetComponent<CharacterSelectMenu>().NewPlayerJoined(newPlayer.GetComponent<NewGamepadInput>());
        } else
        {
            StartCoroutine(UIUtilities.SelectAnchorObject(newPlayer.GetComponent<EventSystem>(), GetCurrentAnchor()));
        }

    }

    public void ButtonTest()
    {
        Debug.Log(gameObject.name + " button was selected.");
    }

    /*
    public void OnEnable()
    {
        if(GamepadInputManager.instance == null || GamepadInputManager.instance.gamepadInputs == null)
        {
            return;
        }

        foreach (NewGamepadInput input in GamepadInputManager.instance.gamepadInputs) {
            if(input == null)
            {
                continue;
            }

            //input.GetComponent<EventSystem>().SetSelectedGameObject(anchorObject);
            StartCoroutine(UIUtilities.SelectAnchorObject(input.GetComponent<EventSystem>(), anchorObject));

        }
    }
    */
}
