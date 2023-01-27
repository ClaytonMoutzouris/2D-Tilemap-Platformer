using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TalentTreeBranch", menuName = "ScriptableObjects/Talents/TalentTreeBranch")]
public class TalentTreeBranch : ScriptableObject
{
    public string description;
    //a bunch of branches?
    public List<Talent> talents;

    public Sprite branchIcon;

    public string GetTooltip()
    {
        string tooltip = "";

        tooltip += name;
        tooltip += "\n" + description;

        return tooltip;
    }
}
