using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum SelectScreenTabIndex { General, Creation, Appearance }
//This is for a single player to select/create their character
public class CharacterSelectScreen : MonoBehaviour
{
    public GameObject anchorObject;
    public List<PlayerMenuTabUI> menuTabs;

    public AppearancePanelUI appearancePanel;
    public int playerIndex = 0;
    public int currentTabIndex = 0;
    public bool playerReady = false;

    // Start is called before the first frame update
    void Start()
    {
        foreach (PlayerMenuTabUI tab in menuTabs)
        {
            tab.SetPlayerIndex(playerIndex);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ConfirmCharacter()
    {
        PlayerCreationData newData = new PlayerCreationData();
        newData.playerColors = appearancePanel.GetColors();
        newData.playerIndex = playerIndex;
        VersusMenuUI.instance.versusGameData.playerDatas[playerIndex] = newData;
        playerReady = true;
        ChangeTab(3);
    }

    public void DeconfirmCreation()
    {
        playerReady = false;
        ChangeTab(1);
    }

    public void StartGame()
    {
        if(CharacterSelectMenu.instance.AllPlayersReady())
        {
            VersusMenuUI.instance.StartGame();
        }
    }

    public void ChangeTab(int index)
    {
        foreach (PlayerMenuTabUI tab in menuTabs)
        {
            tab.CloseTab();
        }

        menuTabs[index].OpenTab(playerIndex);
        currentTabIndex = index;
    }

    public void ChangeTab(SelectScreenTabIndex index)
    {
        foreach (PlayerMenuTabUI tab in menuTabs)
        {
            tab.CloseTab();
        }

        menuTabs[(int)index].OpenTab(playerIndex);
        currentTabIndex = (int)index;

    }


    public void OnEnable()
    {
        //EventSystem.current.SetSelectedGameObject(anchorObject);
        ChangeTab((int)SelectScreenTabIndex.General);
        //UIUtilities.SelectAnchorObject(GamepadInputManager.instance.gamepadInputs[playerIndex].GetEventSystem(), menuTabs[currentTabIndex].anchorObject);

    }

}
