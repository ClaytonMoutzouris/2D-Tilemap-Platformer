﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TalentTreeMenuOption : MonoBehaviour, ISelectHandler
{
    public List<TalentNodeUI> talentNodes = new List<TalentNodeUI>();
    public TalentNodeUI currentNode;
    public TalentsPanelUI talentTreePanel;
    public GameObject container;
    public Button button;
    public ScrollRect scrollRect;

    public TalentNodeUI prefab;

    public void RedirectClick()
    {
        currentNode.button.Select();
    }

    public void AddNodes(List<Talent> talents)
    {
        foreach(Talent t in talents)
        {
            TalentNodeUI node = Instantiate(prefab, container.transform);
            node.SetNode(t, this);

            talentNodes.Add(node);
        }

        currentNode = talentNodes[0];
        SetNav();
    }

    public void AddNode(Talent talent)
    {
        TalentNodeUI node = Instantiate(prefab, container.transform);
        node.SetNode(talent, this);

        talentNodes.Add(node);
        currentNode = talentNodes[0];

        SetNav();
    }

    public void ClearNodes()
    {

    }

    public void LearnNode(TalentNodeUI learnNode)
    {
        talentTreePanel.LearnTalent(learnNode);
    }

    public void UnlearnNode(TalentNodeUI node)
    {
        talentTreePanel.UnlearnTalent(node);

    }

    public void SetCurrentNode(TalentNodeUI node)
    {
        currentNode = node;
        scrollRect.content.localPosition = scrollRect.GetSnapToPositionToBringChildIntoViewHorizontal(currentNode.GetComponent<RectTransform>());

    }

    public void SetNav()
    {
        TalentNodeUI first = null;
        TalentNodeUI last = null;
        TalentNodeUI prev = null;

        foreach (TalentNodeUI node in talentNodes)
        {
            if (!node.gameObject.activeSelf)
            {
                continue;
            }

            Navigation currentNav = node.button.navigation;

            if (first == null)
            {
                first = node;
                Navigation branchNav = button.navigation;
                branchNav.selectOnLeft = node.button;
                button.navigation = branchNav;

            }

            if (prev != null)
            {
                Navigation prevNav = prev.button.navigation;
                prevNav.selectOnRight = node.button;

                currentNav.selectOnLeft = prev.button;
                prev.button.navigation = prevNav;
            }

            currentNav.selectOnUp = button;
            currentNav.selectOnDown = button;


            node.button.navigation = currentNav;


            prev = node;
            last = node;

        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        talentTreePanel.SetCurrentNode(this);
    }
}