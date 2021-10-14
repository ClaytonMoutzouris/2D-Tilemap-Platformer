using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuOptionSelector : MenuOption
{
    public Text nameText;


    //selector stuff
    public ScrollRect scrollRect;
    public GameObject nodesContainer;
    public MenuSelectorNodeUI nodePrefab;
    public MenuColorSelectorNodeUI colorNodePrefab;
    public List<MenuSelectorNodeUI> optionNodes;
    public RectTransform mask;
    public MenuSelectorNodeUI currentNode;

    //public List<string> optionsHeaders;

    void Awake()
    {

        StartCoroutine(SetSize());
    }

    IEnumerator SetSize()
    {
        yield return new WaitForEndOfFrame();

        int sizeAux = nameText.cachedTextGenerator.fontSizeUsedForBestFit;

        nameText.fontSize = sizeAux;
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());

        //Do staff
    }

    public override void Init()
    {
        if(optionNodes.Count <= 0)
        {
            //cant init without nodes
            return;
        }

        SetNavigation();
        SetCurrentNode(0);

    }

    public void Update()
    {
        //scrollRect.ScrollRepositionX(currentNode.GetComponent<RectTransform>());
    }

    public void ClearOptions()
    {
        foreach(MenuSelectorNodeUI selector in optionNodes)
        {
            Destroy(selector.gameObject);
        }

        optionNodes.Clear();
        currentNode = null;
    }

    public void AddOption(string text)
    {

        MenuSelectorNodeUI newNode = Instantiate(nodePrefab, nodesContainer.transform);
        newNode.text.text = text;
        newNode.parent = this;
        optionNodes.Add(newNode);
        
        //When an option is added, we should update the navigation

        //We can assume this is the last item in the list

    }

    public void AddColorOption(Color color)
    {
        MenuColorSelectorNodeUI newNode = Instantiate(colorNodePrefab, nodesContainer.transform);
        newNode.SetColor(color);
        newNode.parent = this;
        optionNodes.Add(newNode);
    }

    public MenuColorSelectorNodeUI GetColorNode()
    {
        if (currentNode is MenuColorSelectorNodeUI colorNode)
        {
            return colorNode;
        }

        return null;
    }

    internal void SelectNode(MenuSelectorNodeUI menuOptionNodeUI)
    {
        throw new NotImplementedException();
    }

    public void SetCurrentNode(int index)
    {
        SetCurrentNode(optionNodes[index]);
    }

    internal void SetCurrentNode(MenuSelectorNodeUI menuOptionNodeUI)
    {
        currentNode = menuOptionNodeUI;

        scrollRect.content.localPosition = scrollRect.GetSnapToPositionToBringChildIntoViewHorizontal(currentNode.GetComponent<RectTransform>());

    }

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);

        if (currentNode == null || button == null)
        {
            return;
        }

        Navigation iNav = button.navigation;

        iNav.selectOnRight = currentNode.button;
        button.navigation = iNav;

    }

    public void RedirectClick()
    {
        currentNode.button.Select();
    }

    public void SetNavigation()
    {
        for (int i = 0; i < optionNodes.Count; i++)
        {

            Navigation customNav = optionNodes[i].GetComponent<Button>().navigation;
            Navigation iNav = button.navigation;

            if (i == 0)
            {
                iNav.selectOnRight = optionNodes[i].GetComponent<Button>();

                customNav.selectOnLeft = optionNodes[optionNodes.Count - 1].GetComponent<Button>();
                if (optionNodes.Count > 1)
                {
                    customNav.selectOnRight = optionNodes[i + 1].GetComponent<Button>();
                }

            }
            else if (i == optionNodes.Count - 1)
            {
                customNav.selectOnRight = optionNodes[0].GetComponent<Button>();
                if (optionNodes.Count > 1)
                {
                    customNav.selectOnLeft = optionNodes[i - 1].GetComponent<Button>();
                }

            }
            else
            {
                customNav.selectOnLeft = optionNodes[i - 1].GetComponent<Button>();
                customNav.selectOnRight = optionNodes[i + 1].GetComponent<Button>();
            }

            customNav.selectOnUp = button;
            customNav.selectOnDown = button;

            button.navigation = iNav;
            optionNodes[i].GetComponent<Button>().navigation = customNav;

        }
    }
}
