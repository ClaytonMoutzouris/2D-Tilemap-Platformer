using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ArcadeMenuTabIndex { MainPanel, GameRules }
public class ArcadeMenu : MenuTabUI
{
    public static ArcadeMenu instance;
    public GameMode gameMode;

    public List<MenuTabUI> menuTabs;
    public ArcadeMenuTabIndex currentTabIndex;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        //
    }

    public void ChangeTab(int tabIndex)
    {
        foreach (MenuTabUI tab in menuTabs)
        {
            tab.CloseTab();
        }

        menuTabs[tabIndex].OpenTab();
        currentTabIndex = (ArcadeMenuTabIndex)tabIndex;
    }

    public void SelectMode(int modeIndex)
    {
        gameMode = (GameMode)modeIndex;
        ChangeTab((int)ArcadeMenuTabIndex.GameRules);

    }

    public override GameObject GetAnchorObject()
    {
        return menuTabs[(int)currentTabIndex].GetAnchorObject();
    }

    public override void CloseTab()
    {
        ChangeTab((int)ArcadeMenuTabIndex.MainPanel);
        gameObject.SetActive(false);
    }

    public override void OpenTab()
    {
        gameObject.SetActive(true);
        ChangeTab((int)ArcadeMenuTabIndex.MainPanel);
    }

}
