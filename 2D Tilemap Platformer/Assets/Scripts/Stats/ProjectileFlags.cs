using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ProjectileFlagType { Piercing, Boomerang, Homing, DestroyOnGround, IgnoreGround, IgnoreGravity, IsAngled, Bounce };
public enum ProjectileAttributesType { ProjectileSpeed, Elasticity, LifeTime };

public class ProjectileFlags
{
    public Dictionary<ProjectileFlagType, ProjectileFlag> flags;

    public ProjectileFlags()
    {

        flags = new Dictionary<ProjectileFlagType, ProjectileFlag>();

        //Create starting stats
        foreach (ProjectileFlagType type in System.Enum.GetValues(typeof(ProjectileFlagType)))
        {
            flags.Add(type, new ProjectileFlag(type));
        }

    }

    public ProjectileFlag GetFlag(ProjectileFlagType type)
    {
        if (flags.ContainsKey(type))
        {
            return flags[type];
        }

        return null;
    }

    public void SetFlag(ProjectileFlag value)
    {
        if (flags.ContainsKey(value.type))
        {
            flags[value.type] = value;
        }
        else
        {
            flags.Add(value.type, value);
        }

    }

    public void AddBonus(ProjectileFlagBonus bonus)
    {
        if (flags.ContainsKey(bonus.type))
        {
            Debug.Log("Adding bonus " + bonus.type + " : " + bonus.bonusValue);
            flags[bonus.type].AddBonus(bonus);
        }
        else
        {
            Debug.Log("Does not contain key " + bonus.type);

        }
    }

    public void RemoveBonus(ProjectileFlagBonus bonus)
    {
        if (bonus == null)
        {
            return;
        }

        if (flags.ContainsKey(bonus.type))
        {
            flags[bonus.type].RemoveBonus(bonus);
        }
    }

    public void AddBonuses(List<ProjectileFlagBonus> bonuses)
    {
        foreach (ProjectileFlagBonus bonus in bonuses)
        {
            AddBonus(bonus);
        }

    }

    public void RemoveBonuses(List<ProjectileFlagBonus> bonuses)
    {
        foreach (ProjectileFlagBonus bonus in bonuses)
        {
            RemoveBonus(bonus);
        }

    }

}

[System.Serializable]
public class ProjectileFlag
{
    public ProjectileFlagType type;
    public bool baseValue = false;
    public List<ProjectileFlagBonus> bonuses;

    public ProjectileFlag(ProjectileFlagType t, bool startingValue = false)
    {
        type = t;
        baseValue = startingValue;
        bonuses = new List<ProjectileFlagBonus>();
    }

    public void AddBonus(ProjectileFlagBonus bonus)
    {
        bonuses.Add(bonus);
    }

    public void RemoveBonus(ProjectileFlagBonus bonus)
    {
        bonuses.Remove(bonus);
    }

    public bool GetBaseValue()
    {
        return baseValue;
    }

    public bool GetValue()
    {
        bool fullValue = baseValue;
        
        //Principles of this, multiple 'true' doesn't mean much, but a 'bonus' of false will overwrite everything else
        //(No trumps all other)


        foreach(ProjectileFlagBonus bonus in bonuses)
        {
            if (bonus.bonusValue)
            {
                fullValue = bonus.bonusValue;
            } else
            {
                //If there is one false in the bonuses, break since no trumps yes
                //Consider layering in the future, like in mtg
                //(Example: something that says you cannot pierce will trump all bonuses that allow you to pierce
                fullValue = bonus.bonusValue;
                break;
            }
        }

        return fullValue;
    }
}

[System.Serializable]
public class ProjectileFlagBonus
{

    //Weapon Class this bonus is for
    public ProjectileFlagType type;
    public bool bonusValue = true;

    public ProjectileFlagBonus(ProjectileFlagType t, bool val)
    {
        type = t;
        bonusValue = val;
    }

}