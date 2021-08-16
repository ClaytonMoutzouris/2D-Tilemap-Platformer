using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum VersusGameMode { Arena_Battle, King_Of_The_Hill, Capture_The_Flag }
public enum MenuOptionIndex { Mode, Lives, TimeLimit, Map }
public class VersusMenuUI : MonoBehaviour
{
    public static VersusMenuUI instance;
    public VersusGameMode gameMode;
    public int lives;
    public int timeLimit;
    public string mapName;
    public GameData versusGameData;
    public VersusMenuOptions versusMenuOptions;

    public List<MenuOptionSelectorUI> menuOptions;

    public GameObject optionsContainer;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        versusGameData.playerDatas = new PlayerCreationData[4];
        SetNavigation();

        //TODO: Should I create and add all the menu options here? Not a bad idea.

        LoadMenuOptions();
    }

    public void LoadMenuOptions()
    {
        if(versusMenuOptions == null)
        {
            //this should never happen
            return;
        }

        foreach (VersusGameMode gameMode in versusMenuOptions.gameModes)
        {
            menuOptions[(int)MenuOptionIndex.Mode].AddOption(gameMode.ToString());
        }
        menuOptions[(int)MenuOptionIndex.Mode].Init();


        foreach (int numLives in versusMenuOptions.livesOptions)
        {
            menuOptions[(int)MenuOptionIndex.Lives].AddOption(numLives.ToString());
        }

        menuOptions[(int)MenuOptionIndex.Lives].Init();


        foreach (int limit in versusMenuOptions.timeLimits)
        {
            menuOptions[(int)MenuOptionIndex.TimeLimit].AddOption(limit.ToString());
        }

        menuOptions[(int)MenuOptionIndex.TimeLimit].Init();


        Debug.Log("About to load maps");
        versusMenuOptions.LoadMaps();

        foreach (string map in versusMenuOptions.maps)
        {
            menuOptions[(int)MenuOptionIndex.Map].AddOption(map);
        }

        menuOptions[(int)MenuOptionIndex.Map].Init();
    }

    public void ConfirmSelections()
    {
        gameMode = (VersusGameMode)System.Enum.Parse(typeof(VersusGameMode), menuOptions[(int)MenuOptionIndex.Mode].currentNode.text.text);
        lives = int.Parse(menuOptions[(int)MenuOptionIndex.Lives].currentNode.text.text);

        timeLimit = int.Parse(menuOptions[(int)MenuOptionIndex.TimeLimit].currentNode.text.text);

        mapName = menuOptions[(int)MenuOptionIndex.Map].currentNode.text.text;

        versusGameData.gameMode = gameMode;
        versusGameData.lives = lives;
        versusGameData.timeLimit = timeLimit;
        versusGameData.mapName = mapName;

        MainMenu.instance.ChangeTab(2);

    }

    public void StartGame()
    {
        SceneManager.LoadScene("GambleArena");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    public void SetNavigation()
    {
        for (int i = 0; i < menuOptions.Count; i++)
        {

            Navigation customNav = menuOptions[i].button.navigation;

            if (i == 0)
            {
                //I need to save this for later if i want to loop from top to bottom on options
                //customNav.selectOnUp = interactableUp;
                customNav.selectOnDown = menuOptions[i + 1].GetComponent<Button>();

            }
            else if (i == menuOptions.Count - 1)
            {
                customNav.selectOnUp = menuOptions[i - 1].GetComponent<Button>();
            }
            else
            {
                customNav.selectOnUp = menuOptions[i - 1].GetComponent<Button>();
                customNav.selectOnDown = menuOptions[i + 1].GetComponent<Button>();
            }


            menuOptions[i].GetComponent<Button>().navigation = customNav;

        }
    }
    
}
