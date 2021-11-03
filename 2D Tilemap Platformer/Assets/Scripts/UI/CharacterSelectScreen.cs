using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SelectScreenTabIndex { General, Creation, Appearance, Talents, Confirmed }
//This is for a single player to select/create their character
public class CharacterSelectScreen : MonoBehaviour
{
    public GameObject anchorObject;
    public List<PlayerMenuTabUI> menuTabs;

    public AppearancePanelUI appearancePanel;
    public CharacterSelectPortrait portrait;
    public TalentsPanelUI talentPanel;
    public StatsPanelUI statsPanel;
    public int playerIndex = 0;
    public int currentTabIndex = 0;
    public bool playerReady = false;
    public float backoutTime = 2;

    public BackoutBar backoutBar;

    // Start is called before the first frame update

    public void InitScreen()
    {
        foreach (PlayerMenuTabUI tab in menuTabs)
        {
            tab.SetPlayerIndex(playerIndex);
        }
        backoutBar.SetBar(0);
        portrait.LoadPortrait();
        //appearancePanel.LoadMenuOptions();
        appearancePanel.LoadColors();
        statsPanel.LoadStats();
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();

        //portrait.colorSwap.SetBaseColors(appearancePanel.GetColors());

    }

    public void HandleInput()
    {
        if(GamepadInputManager.instance.gamepadInputs[playerIndex] == null)
        {
            //Only do this if a player is actually joined
            return;
        }

        if(GamepadInputManager.instance.gamepadInputs[playerIndex].buttonInputs[(int)GamepadButtons.EastButton])
        {
            //CharacterSelectMenu.instance.DropPlayer(playerIndex);
            StartCoroutine(BackOut());
        }

    }

    public IEnumerator BackOut()
    {
        float backoutTimestamp = Time.time;


        while(GamepadInputManager.instance.gamepadInputs[playerIndex].buttonInputs[(int)GamepadButtons.EastButton])
        {
            float percent = Mathf.Clamp01(Mathf.Abs(Time.time - backoutTimestamp) / backoutTime);
            backoutBar.SetBar(percent);
            if (Time.time >= backoutTimestamp + backoutTime)
            {
                MainMenu.instance.ChangeTab((int)MainMenuTabIndex.ArcadeMenu);
                break;
            }


            yield return null;

        }

        backoutBar.SetBar(0);

    }

    public void ConfirmCharacter()
    {
        PlayerCreationData newData = new PlayerCreationData();
        newData.playerColors = appearancePanel.colors;
        newData.talents = talentPanel.learnedTalents;
        newData.startingStats = statsPanel.stats;
        newData.playerIndex = playerIndex;
        ArcadeGameRulesMenu.instance.arcadeGameData.playerDatas[playerIndex] = newData;
        playerReady = true;
        ChangeTab(4);
    }

    public void DeconfirmCreation()
    {
        playerReady = false;
        ChangeTab(1);
    }

    public void DropOut()
    {
        CharacterSelectMenu.instance.DropPlayer(playerIndex);
    }

    public void StartGame()
    {
        if(CharacterSelectMenu.instance.AllPlayersReady())
        {
            GameManager.instance.StartGame();
        }
    }

    public void ChangeTab(int index)
    {
        foreach (PlayerMenuTabUI tab in menuTabs)
        {
            tab.CloseTab();
        }

        menuTabs[index].OpenTab();
        currentTabIndex = index;
    }

    public void ChangeTab(SelectScreenTabIndex index)
    {
        foreach (PlayerMenuTabUI tab in menuTabs)
        {
            tab.CloseTab();
        }

        menuTabs[(int)index].OpenTab();
        currentTabIndex = (int)index;

    }


    public void OnEnable()
    {
        InitScreen();
        //EventSystem.current.SetSelectedGameObject(anchorObject);
        ChangeTab((int)SelectScreenTabIndex.General);
        //UIUtilities.SelectAnchorObject(GamepadInputManager.instance.gamepadInputs[playerIndex].GetEventSystem(), menuTabs[currentTabIndex].anchorObject);

    }

}
