using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TalentNodeUI : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public Button button;
    public Talent talent;
    public bool learned = false;
    public TalentTreeMenuOption branch;
    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
    }

    public void SetNode(Talent talent, TalentTreeMenuOption branch)
    {
        this.talent = Instantiate(talent);
        this.branch = branch;
    }

    public void SelectOption()
    {
        if(talent == null)
        {
            return;
        }

        branch.talentTreePanel.TalentNodeSelected(this);

    }

    public void SetLearned(bool isLearned)
    {
        learned = isLearned;

        if(learned)
        {
            button.image.color = Color.yellow;
        }
        else
        {
            button.image.color = Color.white;
        }
    }

    public void Confirm()
    {
        if (branch == null)
        {
            return;
        }
        //branch.button.Select();
    }

    public void OnSelect(BaseEventData eventData)
    {
        branch.talentTreePanel.tooltip.DisplayTooltip(talent.GetTooltip());
        branch.SetCurrentNode(this);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        branch.talentTreePanel.tooltip.ClearTooltip();
    }

}
