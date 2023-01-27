using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class ClassSelectPanelUI : UIScrollMenu
{
    public ClassSelectOption prefab;
    public PlayerMenuTabUI menuTabUI;

    public CharacterSelectScreen characterScreen;

    public List<ClassData> classes;


    public void Update()
    {

    }

    public override void LoadMenuOptions()
    {
        ClearMenuOptions();

        foreach (ClassData classData in classes)
        {
            ClassSelectOption temp = Instantiate(prefab, container.transform) as ClassSelectOption;
            temp.menuParent = this;
            temp.SetClass(classData);
            AddOption(temp);
        }

        AddOption(backButton);
        backButton.transform.SetAsLastSibling();

        SetNavigation();
        SetCurrentNode(0);
        menuTabUI.anchorObject = currentNode.gameObject;
        menuTabUI.SetAnchor();
    }

    public void SelectClass(ClassData classData)
    {
        characterScreen.selectedClass = Instantiate(classData);

        characterScreen.ChangeTab(1);
    }

}
