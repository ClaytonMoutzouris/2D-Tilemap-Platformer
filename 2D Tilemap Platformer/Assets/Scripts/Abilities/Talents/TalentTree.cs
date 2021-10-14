using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TalentTree", menuName = "ScriptableObjects/Talents/TalentTree")]
public class TalentTree : ScriptableObject
{
    //a bunch of branches?

    public List<TalentTreeBranch> talentTreeBranches;
}
