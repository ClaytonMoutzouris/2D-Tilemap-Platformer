using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TalentsPanelUI : MonoBehaviour
{
    public CharacterSelectScreen selectScreen;
    public GameObject container;
    public List<TalentTreeMenuOption> branches;
    public MenuTabUI menuTab;
    public Button backButton;
    public ScrollRect scrollRect;

    public TooltipDisplay tooltip;
    public TalentTreeMenuOption currentBranch;

    //Tester
    public TalentTree talentTree;
    public List<Talent> learnedTalents;

    public TalentTreeMenuOption branchPrefab;

    public void Start()
    {
        LoadTalentTree(talentTree);
        menuTab = GetComponent<MenuTabUI>();
        SetNav();
    }

    public void LoadTalentTree(TalentTree talentTree)
    {
        this.talentTree = talentTree;

        foreach(TalentTreeBranch branch in talentTree.talentTreeBranches)
        {
            TalentTreeMenuOption menuBranch = Instantiate(branchPrefab, container.transform);
            menuBranch.talentTreePanel = this;
            menuBranch.AddNodes(branch.talents);

            branches.Add(menuBranch);


        }
    }

    public void LearnTalent(TalentNodeUI talentNode)
    {
        learnedTalents.Add(talentNode.talent);
    }

    public void UnlearnTalent(TalentNodeUI talentNode)
    {
        if(!learnedTalents.Contains(talentNode.talent))
        {
            Debug.Log("Doesnt contain the talent");
            return;
        }
        learnedTalents.Remove(talentNode.talent);
    }

    public void SetNav()
    {
        TalentTreeMenuOption first = null;
        TalentTreeMenuOption last = null;
        TalentTreeMenuOption prev = null;

        foreach (TalentTreeMenuOption branch in branches)
        {
            if (!branch.gameObject.activeSelf)
            {
                continue;
            }

            Navigation currentNav = branch.button.navigation;

            if (first == null)
            {
                first = branch;

                Navigation backNav = backButton.navigation;
                backNav.selectOnDown = first.button;

                Navigation firstNav = first.button.navigation;
                firstNav.selectOnUp = backButton;

                backButton.navigation = backNav;
                //backButton.transform.SetAsLastSibling();
                first.button.navigation = firstNav;
            } else if (prev != null)
            {

                Navigation prevNav = prev.button.navigation;
                prevNav.selectOnDown = branch.button;

                currentNav.selectOnUp = prev.button;
                prev.button.navigation = prevNav;

                branch.button.navigation = currentNav;

            }



            prev = branch;
            last = branch;

        }

        if(last != null)
        {
            Navigation backNav = backButton.navigation;
            backNav.selectOnUp = last.button;

            Navigation lastNav = last.button.navigation;
            lastNav.selectOnDown = backButton;

            backButton.navigation = backNav;
            //backButton.transform.SetAsLastSibling();
            last.button.navigation = lastNav;



        }

    }

    public void SetCurrentNode(TalentTreeMenuOption branch)
    {
        currentBranch = branch;
        //scrollRect.GetSnapToPositionToBringChildIntoView(currentBranch.transform);
        scrollRect.content.localPosition = scrollRect.GetSnapToPositionToBringChildIntoViewVertical(currentBranch.GetComponent<RectTransform>());
        //scrollRect.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, currentNode.GetComponent<RectTransform>().sizeDelta.x);
        //mask.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, currentNode.GetComponent<RectTransform>().sizeDelta.x);

    }

}
