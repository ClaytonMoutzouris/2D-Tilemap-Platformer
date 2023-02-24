using System.Collections.Generic;
using UnityEngine;

public enum ClassType { Medic, BountyHunter, Soldier, Engineer };

[CreateAssetMenu(fileName = "ClassData", menuName = "ScriptableObjects/Classes/ClassData")]
public class ClassData : ScriptableObject
{
    [Header("Class Info")]
    public ClassType classType;
    public string classDescription;

    [Header("Talents")]
    public TalentTree talents;

    [Header("Stat Bonus")]
    public List<StatBonus> statBonuses = new List<StatBonus>();

    [Header("Starting Equipment")]
    public Weapon startingWeapon;
    public RangedWeapon startingRanged;
    public Gadget startingGadget;
    //public List<Equipment> startingEquipment;
    public List<ItemData> startingItems = new List<ItemData>();


    public string GetTooltip()
    {
        string tooltip = "";
        tooltip += classDescription;
        if(startingWeapon != null)
        {
            tooltip += "\n" + "<color=#" + ColorUtility.ToHtmlStringRGB(Color.red) + ">" + startingWeapon.name + "</color>";
        }

        if (startingRanged != null)
        {
            tooltip += "\n" + "<color=#" + ColorUtility.ToHtmlStringRGB(Color.red) + ">" + startingRanged.name + "</color>";
        }

        if (startingGadget != null)
        {
            tooltip += "\n" + "<color=#" + ColorUtility.ToHtmlStringRGB(Color.magenta) + ">" + startingGadget.name + "</color>";
        }

        foreach (StatBonus bonus in statBonuses)
        {
            tooltip += "\n" + "<color=#" + ColorUtility.ToHtmlStringRGB(Color.green) + ">" + bonus.GetTooltip() + "</color>";
        }

        foreach(ItemData item in startingItems)
        {
            tooltip += "\n" + "<color=#" + ColorUtility.ToHtmlStringRGB(Color.yellow) + ">" + item.name + "</color>";
        }

        return tooltip;
    }
}
