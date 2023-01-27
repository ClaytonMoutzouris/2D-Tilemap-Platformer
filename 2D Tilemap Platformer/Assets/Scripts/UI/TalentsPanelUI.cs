using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    public TalentTreeMenuOption currentBranch;

    //Tester
    public TalentTree talentTree;
    public List<Talent> learnedTalents;

    public TalentTreeMenuOption branchPrefab;

    public int talentPoints = 0;
    public TextMeshProUGUI talentPointsDisplay;

    public void OnEnable()
    {
        talentPoints = ArcadeGameRulesMenu.instance.arcadeGameData.talentPoints;
        LoadTalentTree(selectScreen.selectedClass.talents);

        talentPoints -= learnedTalents.Count;
        UpdateTalentPointsDisplay();

    }

    public void Start()
    {
        menuTab = GetComponent<MenuTabUI>();
        SetNav();
    }

    public void ClearBranches()
    {
        foreach(TalentTreeMenuOption branch in branches)
        {
            Destroy(branch.gameObject);
        }

        branches.Clear();
    }

    public void LoadTalentTree(TalentTree talentTree)
    {
        this.talentTree = talentTree;

        ClearBranches();

        foreach (TalentTreeBranch branch in talentTree.talentTreeBranches)
        {
            TalentTreeMenuOption menuBranch = Instantiate(branchPrefab, container.transform);
            menuBranch.talentTreePanel = this;
            menuBranch.SetBranch(branch);

            branches.Add(menuBranch);


        }

                SetNav();

    }

    public void TalentNodeSelected(TalentNodeUI talentNode)
    {
        if(talentNode.learned)
        {
            if(UnlearnTalent(talentNode.talent))
            {
                talentNode.SetLearned(false);
                talentPoints++;
                UpdateTalentPointsDisplay();
            }
        } else
        {
            if(LearnTalent(talentNode.talent))
            {
                talentNode.SetLearned(true);
                talentPoints--;
                UpdateTalentPointsDisplay();
            }
        }
    }

    public void UpdateTalentPointsDisplay()
    {
        talentPointsDisplay.text = "Talent Points: " + talentPoints;
    }

    public bool LearnTalent(Talent talent)
    {
        if (talentPoints > 0) { 
            learnedTalents.Add(talent);
            return true;
        }

        return false;
    }

    public bool UnlearnTalent(Talent talent)
    {
        if(!learnedTalents.Contains(talent))
        {
            Debug.Log("Doesnt contain the talent, how did this happen?");
            return false;
        }

        
        return learnedTalents.Remove(talent);
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
