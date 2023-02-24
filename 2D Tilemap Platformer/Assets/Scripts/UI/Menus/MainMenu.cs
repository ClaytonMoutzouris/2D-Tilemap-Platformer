using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public enum MainMenuTabIndex { MainMenu, ArcadeMenu, CharacterSelectMenu }
public class MainMenu : MonoBehaviour
{
    public static MainMenu instance;
    public List<MenuTabUI> menuTabs;
    // This should change based on the current menu tab that is active
    //(will need to be more elegant for menu tabs that seperate for each player)
    public MainMenuTabIndex currentTabIndex;
    //public MenuTabUI menuTab;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        ChangeTab((int)MainMenuTabIndex.MainMenu);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeTab(int tabIndex)
    {
        foreach(MenuTabUI tab in menuTabs)
        {
            tab.CloseTab();
        }

        menuTabs[tabIndex].OpenTab();
        currentTabIndex = (MainMenuTabIndex)tabIndex;
    }

    public GameObject GetCurrentAnchor()
    {
        return menuTabs[(int)currentTabIndex].GetAnchorObject();
        //return currentAnchor;
    }

    public void NewPlayerJoined(PlayerInput newPlayer)
    {
        if(currentTabIndex == MainMenuTabIndex.CharacterSelectMenu)
        {
            menuTabs[(int)currentTabIndex].GetComponent<CharacterSelectMenu>().NewPlayerJoined(newPlayer.playerIndex);
        } else
        {
            StartCoroutine(UIUtilities.SelectAnchorObject(newPlayer.GetComponent<EventSystem>(), GetCurrentAnchor()));
        }

    }

    public void ButtonTest()
    {
        Debug.Log(gameObject.name + " button was selected.");
    }

    public void ExitGame()
    {
        Application.Quit();
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
