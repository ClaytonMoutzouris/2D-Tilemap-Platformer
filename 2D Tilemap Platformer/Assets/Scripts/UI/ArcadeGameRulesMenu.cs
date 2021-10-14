using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameMode { FreePlay, Versus, Coop }
public enum MenuOptionIndex { Confirm, Back, Map, Lives, TimeLimit, Count }
public class ArcadeGameRulesMenu : UIScrollMenu
{
    public static ArcadeGameRulesMenu instance;
    public ArcadeMenu arcadeMenu;
    public GameData arcadeGameData;
    public ArcadeMenuOptions menuOptionsData;
    //If we want to instantiate all the option objects, we would need these
    public MenuOptionSelector optionSelectorPrefab;
    public MenuTabUI menuTab;
    //public List<MenuOptionSelectorUI> options = new List<MenuOptionSelectorUI>();
    //public Dictionary<MenuOptionIndex, MenuOptionSelector> optionDictionary = new Dictionary<MenuOptionIndex, MenuOptionSelector>();
    public GameMode gameMode;

    private void Awake()
    {
        instance = this;
    }

    public void OnEnable()
    {
        //LoadMode(ArcadeMenu.instance.gameMode);

    }

    public override void LoadMenuOptions()
    {
        base.LoadMenuOptions();

        MenuOptionSelector mapOptions = Instantiate(optionSelectorPrefab, container.transform);

        menuOptionsData.LoadOptions();

        foreach (string map in menuOptionsData.maps)
        {
            mapOptions.AddOption(map);
        }

        mapOptions.Init();
        AddOption(mapOptions);

        MenuOptionSelector livesOptions = Instantiate(optionSelectorPrefab, container.transform);

        foreach (int lives in menuOptionsData.livesOptions)
        {
            livesOptions.AddOption(lives.ToString());
        }

        livesOptions.Init();
        AddOption(livesOptions);

        MenuOptionSelector timerOptions = Instantiate(optionSelectorPrefab, container.transform);

        foreach (int time in menuOptionsData.timeLimits)
        {
            timerOptions.AddOption(time.ToString());
        }

        timerOptions.Init();
        AddOption(timerOptions);

        SetNavigation();
        SetCurrentNode(0);
        GetComponent<MenuTabUI>().anchorObject = currentNode.gameObject;
        menuTab.SetAnchor();
        LoadMode(arcadeMenu.gameMode);
    }

    public void LoadMode(GameMode mode)
    {
        gameMode = mode;
        Debug.Log("Loading Mode: " + mode);
        //Do something about loading the modes here

        //clear the previous options out

        switch(mode)
        {
            case GameMode.FreePlay:
                LoadFreePlayMode();
                break;
            case GameMode.Versus:
                LoadVersusMode();
                break;
            case GameMode.Coop:
                LoadFreePlayMode();
                break;
        }

        
        SetNavigation();
    }

    public void LoadFreePlayMode()
    {
        //create the option objects and then set them
        foreach(MenuOption option in nodes)
        {
            option.gameObject.SetActive(false);
        }

        nodes[(int)MenuOptionIndex.Confirm].gameObject.SetActive(true);
        nodes[(int)MenuOptionIndex.Back].gameObject.SetActive(true);
        nodes[(int)MenuOptionIndex.Map].gameObject.SetActive(true);
    }

    public void LoadVersusMode()
    {
        //create the option objects and then set them
        nodes[(int)MenuOptionIndex.Confirm].gameObject.SetActive(true);
        nodes[(int)MenuOptionIndex.Back].gameObject.SetActive(true);
        nodes[(int)MenuOptionIndex.Map].gameObject.SetActive(true);
        nodes[(int)MenuOptionIndex.Lives].gameObject.SetActive(true);
        nodes[(int)MenuOptionIndex.TimeLimit].gameObject.SetActive(true);

    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void SetCurrentNode(MenuOption node)
    {
        base.SetCurrentNode(node);

        switch((MenuOptionIndex)node.optionIndex)
        {
            case MenuOptionIndex.Map:
                arcadeGameData.mapName = ((MenuOptionSelector)node).currentNode.text.text;
                break;
            case MenuOptionIndex.Lives:
                arcadeGameData.lives = int.Parse(((MenuOptionSelector)node).currentNode.text.text); 
                break;
            case MenuOptionIndex.TimeLimit:
                arcadeGameData.timeLimit = int.Parse(((MenuOptionSelector)node).currentNode.text.text);
                break;
        }
    }


    public void ConfirmArcadeData()
    {

        arcadeGameData.gameMode = gameMode;
        //arcadeGameData.lives = int.Parse(menuOptions[(int)MenuOptionIndex.Lives].currentNode.text.text);
        //arcadeGameData.timeLimit = int.Parse(menuOptions[(int)MenuOptionIndex.TimeLimit].currentNode.text.text);

        MainMenu.instance.ChangeTab((int)MainMenuTabIndex.CharacterSelectMenu);
    }

}
