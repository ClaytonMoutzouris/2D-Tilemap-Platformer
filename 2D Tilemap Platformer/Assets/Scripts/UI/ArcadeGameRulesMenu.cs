using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameMode { FreePlay, Versus, Coop }
public enum MenuOptionIndex { Confirm, Back, Map, Lives, TimeLimit, TalentPoints, StatPoints, Tier, Count }
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

    public override void LoadMenuOptions()
    {
        base.LoadMenuOptions();
        Debug.Log("Loading Menu Options");

        MenuOptionSelector mapOptions = Instantiate(optionSelectorPrefab, container.transform);

        menuOptionsData.LoadOptions();

        foreach (string map in menuOptionsData.maps)
        {
            mapOptions.AddOption(map);
        }

        mapOptions.Init();
        mapOptions.SetName("Map");
        AddOption(mapOptions);

        MenuOptionSelector livesOptions = Instantiate(optionSelectorPrefab, container.transform);

        foreach (int lives in menuOptionsData.livesOptions)
        {
            livesOptions.AddOption(lives.ToString());
        }

        livesOptions.Init();
        livesOptions.SetName("Lives");

        AddOption(livesOptions);

        MenuOptionSelector timerOptions = Instantiate(optionSelectorPrefab, container.transform);

        foreach (int time in menuOptionsData.timeLimits)
        {
            timerOptions.AddOption(time.ToString());
        }

        timerOptions.Init();
        timerOptions.SetName("Time Limit");
        AddOption(timerOptions);

        MenuOptionSelector talentPointsOptions = Instantiate(optionSelectorPrefab, container.transform);

        foreach (int talentPoints in menuOptionsData.talentPoints)
        {
            talentPointsOptions.AddOption(talentPoints.ToString());
        }

        talentPointsOptions.Init();
        talentPointsOptions.SetName("Talent Points");
        AddOption(talentPointsOptions);

        MenuOptionSelector statPointsOptions = Instantiate(optionSelectorPrefab, container.transform);

        foreach (int statPoints in menuOptionsData.statPoints)
        {
            statPointsOptions.AddOption(statPoints.ToString());
        }

        statPointsOptions.Init();
        statPointsOptions.SetName("Stat Points");
        AddOption(statPointsOptions);

        //Tier Level
        MenuOptionSelector levelTierOptions = Instantiate(optionSelectorPrefab, container.transform);

        foreach (int tier in menuOptionsData.levelTier)
        {
            levelTierOptions.AddOption(tier.ToString());
        }

        levelTierOptions.Init();
        levelTierOptions.SetName("Level Tier");
        AddOption(levelTierOptions);

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
        nodes[(int)MenuOptionIndex.Tier].gameObject.SetActive(true);
        nodes[(int)MenuOptionIndex.TalentPoints].gameObject.SetActive(false);
        nodes[(int)MenuOptionIndex.StatPoints].gameObject.SetActive(false);

    }

    public void LoadVersusMode()
    {
        foreach (MenuOption option in nodes)
        {
            option.gameObject.SetActive(false);
        }
        //create the option objects and then set them
        nodes[(int)MenuOptionIndex.Confirm].gameObject.SetActive(true);
        nodes[(int)MenuOptionIndex.Back].gameObject.SetActive(true);
        nodes[(int)MenuOptionIndex.Map].gameObject.SetActive(true);
        nodes[(int)MenuOptionIndex.Lives].gameObject.SetActive(true);
        nodes[(int)MenuOptionIndex.TimeLimit].gameObject.SetActive(true);
        nodes[(int)MenuOptionIndex.Tier].gameObject.SetActive(true);
        nodes[(int)MenuOptionIndex.TalentPoints].gameObject.SetActive(false);
        nodes[(int)MenuOptionIndex.StatPoints].gameObject.SetActive(false);

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
            case MenuOptionIndex.TalentPoints:
                arcadeGameData.talentPoints = int.Parse(((MenuOptionSelector)node).currentNode.text.text);
                break;
            case MenuOptionIndex.Tier:
                arcadeGameData.levelTier = int.Parse(((MenuOptionSelector)node).currentNode.text.text);
                break;
            case MenuOptionIndex.StatPoints:
                arcadeGameData.statPoints = int.Parse(((MenuOptionSelector)node).currentNode.text.text);
                break;
        }
    }


    public void ConfirmArcadeData()
    {

        arcadeGameData.gameMode = gameMode;

        switch(arcadeGameData.levelTier)
        {
            case 0:
                arcadeGameData.statPoints = 0;
                arcadeGameData.talentPoints = 0;
                break;
            case 1:
                arcadeGameData.statPoints = 2;
                arcadeGameData.talentPoints = 1;
                break;
            case 2:
                arcadeGameData.statPoints = 4;
                arcadeGameData.talentPoints = 3;
                break;
            case 3:
                arcadeGameData.statPoints = 7;
                arcadeGameData.talentPoints = 5;
                break;
            case 4:
                arcadeGameData.statPoints = 10;
                arcadeGameData.talentPoints = 8;
                break;

            default:
                arcadeGameData.statPoints = 0;
                arcadeGameData.talentPoints = 0;
                break;
        }
        //arcadeGameData.lives = int.Parse(menuOptions[(int)MenuOptionIndex.Lives].currentNode.text.text);
        //arcadeGameData.timeLimit = int.Parse(menuOptions[(int)MenuOptionIndex.TimeLimit].currentNode.text.text);

        MainMenu.instance.ChangeTab((int)MainMenuTabIndex.CharacterSelectMenu);
    }

}
