﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stats : MonoBehaviour
{

    public Dictionary<StatType, Stat> primaryStats;
    public Dictionary<SecondaryStatType, SecondaryStat> secondaryStats;
    public List<WeaponAttributeBonus> weaponBonuses;
    public Stat[] startingStats = new Stat[(int)StatType.Luck+1];
    public SecondaryStat[] startingSecondaryStats = new SecondaryStat[(int)SecondaryStatType.DamageReduction+1];
    public Dictionary<AbilityFlagType, AbilityFlag> abilityFlags;



    public void Start()
    {

        /*
        secondaryStats.Add(SecondaryStatType.BaseHealth, new SecondaryStat(SecondaryStatType.BaseHealth, this, new List<StatDependancy> { new StatDependancy(StatType.Constitution, 10) }, 50));
        secondaryStats.Add(SecondaryStatType.AttackSpeedBonus, new SecondaryStat(SecondaryStatType.AttackSpeedBonus, this, new List<StatDependancy> { new StatDependancy(StatType.Speed, 1) }, 0));
        secondaryStats.Add(SecondaryStatType.CritChance, new SecondaryStat(SecondaryStatType.CritChance, this, new List<StatDependancy> { new StatDependancy(StatType.Luck, 1) }, 5));
        secondaryStats.Add(SecondaryStatType.CritDamage, new SecondaryStat(SecondaryStatType.CritDamage, this, new List<StatDependancy> { }, 0));
        secondaryStats.Add(SecondaryStatType.DodgeChance, new SecondaryStat(SecondaryStatType.DodgeChance, this, new List<StatDependancy> { new StatDependancy(StatType.Luck, 1) }, 5));
        secondaryStats.Add(SecondaryStatType.ExtraJumps, new SecondaryStat(SecondaryStatType.ExtraJumps, this, new List<StatDependancy> { }, 0));
        secondaryStats.Add(SecondaryStatType.MoveSpeed, new SecondaryStat(SecondaryStatType.MoveSpeed, this, new List<StatDependancy> { new StatDependancy(StatType.Speed, 2.5f) }, 0));
        secondaryStats.Add(SecondaryStatType.DamageBonus, new SecondaryStat(SecondaryStatType.DamageBonus, this, new List<StatDependancy> { new StatDependancy(StatType.Attack, 5) }, 0));
        secondaryStats.Add(SecondaryStatType.DamageReduction, new SecondaryStat(SecondaryStatType.DamageReduction, this, new List<StatDependancy> { new StatDependancy(StatType.Defense, 5) }, 0));
        */

    }

    public void Initialize()
    {

        primaryStats = new Dictionary<StatType, Stat>();
        foreach (Stat stat in startingStats)
        {
            primaryStats.Add(stat.type, stat);
        }

        secondaryStats = new Dictionary<SecondaryStatType, SecondaryStat>();
        foreach (SecondaryStat secondaryStat in startingSecondaryStats)
        {
            secondaryStat.stats = this;
            secondaryStats.Add(secondaryStat.type, secondaryStat);
        }

        abilityFlags = new Dictionary<AbilityFlagType, AbilityFlag>();
        foreach (AbilityFlagType flag in System.Enum.GetValues(typeof(AbilityFlagType)))
        {
            abilityFlags.Add(flag, new AbilityFlag(flag));
        }

        //weaponBonuses = new List<WeaponAttributeBonus>();

    }

    public void SetStats(List<Stat> stats)
    {
        //this.primaryStats.Clear();

        foreach (Stat stat in stats)
        {
            this.primaryStats[stat.type] = stat;
        }

    }

    public Stat GetStat(StatType type)
    {
        return primaryStats[type];
    }

    public SecondaryStat GetSecondaryStat(SecondaryStatType type)
    {
        return secondaryStats[type];
    }

    public AbilityFlag GetAbilityFlag(AbilityFlagType type)
    {
        return abilityFlags[type];
    }

    public void AddPrimaryBonus(StatBonus bonus)
    {
        primaryStats[bonus.type].AddBonus(bonus);
    }

    public void RemovePrimaryBonus(StatBonus bonus)
    {
        primaryStats[bonus.type].RemoveBonus(bonus);
    }

    public void AddPrimaryBonuses(List<StatBonus> bonuses)
    {
        foreach (StatBonus bonus in bonuses)
        {
            primaryStats[bonus.type].AddBonus(bonus);
        }

    }

    public void RemovePrimaryBonuses(List<StatBonus> bonuses)
    {
        foreach (StatBonus bonus in bonuses)
        {
            primaryStats[bonus.type].RemoveBonus(bonus);
        }

    }

    public void AddSecondaryBonus(SecondaryStatBonus bonus)
    {
        secondaryStats[bonus.type].AddBonus(bonus);
    }

    public void RemoveSecondaryBonus(SecondaryStatBonus bonus)
    {
        secondaryStats[bonus.type].RemoveBonus(bonus);
    }

    public void AddSecondaryBonuses(List<SecondaryStatBonus> bonuses)
    {
        foreach (SecondaryStatBonus bonus in bonuses)
        {
            secondaryStats[bonus.type].AddBonus(bonus);
        }

    }

    public void RemoveSecondaryBonuses(List<SecondaryStatBonus> bonuses)
    {
        foreach (SecondaryStatBonus bonus in bonuses)
        {
            secondaryStats[bonus.type].RemoveBonus(bonus);
        }

    }

    public void AddAbilityFlagBonus(AbilityFlagBonus bonus)
    {
        abilityFlags[bonus.type].AddBonus(bonus);
    }

    public void RemoveAbilityFlagBonus(AbilityFlagBonus bonus)
    {
        abilityFlags[bonus.type].RemoveBonus(bonus);
    }

    public void AddAbilityFlagBonuses(List<AbilityFlagBonus> bonuses)
    {
        foreach (AbilityFlagBonus bonus in bonuses)
        {
            abilityFlags[bonus.type].AddBonus(bonus);
        }

    }

    public void RemoveAbilityFlagBonuses(List<AbilityFlagBonus> bonuses)
    {
        foreach (AbilityFlagBonus bonus in bonuses)
        {
            abilityFlags[bonus.type].RemoveBonus(bonus);
        }

    }

}
//These should only be INTEGERS
public enum StatType { Attack, Defense, Constitution, Speed, Luck }
//These should all be floats, to make sure it covers any kind of number we need
public enum SecondaryStatType { MoveSpeed, BaseHealth, JumpHeight, CritChance, CritDamage, AttackSpeedBonus, DodgeChance, ExtraJumps, DamageBonus, DamageReduction }

public static class StatTypeMethods
{
    public static string GetShortName(StatType type)
    {
        switch (type)
        {
            case StatType.Attack:
                return "ATK";
            case StatType.Defense:
                return "DEF";
            case StatType.Constitution:
                return "CON";
            case StatType.Speed:
                return "SPD";
            case StatType.Luck:
                return "LCK";
        }

        //Should never be reached unless we are trying to cram an enum that doesnt exist
        return "";
    }
}

[System.Serializable]
public class Stat
{
    public StatType type;
    public int value;
    [HideInInspector]
    public List<StatBonus> bonuses;
    //Refence to the parent stats, mostly for secondary stats

    public Stat(StatType t, int startingValue = 5)
    {
        type = t;
        value = startingValue;
        bonuses = new List<StatBonus>();
    }

    public void AddBonus(StatBonus bonus)
    {
        bonuses.Add(bonus);
    }

    public void RemoveBonus(StatBonus bonus)
    {
        bonuses.Remove(bonus);
    }

    public int GetBaseValue()
    {
        return value;
    }

    public virtual int GetValue()
    {
        float fullValue = value;
        float multiplier = 0;

        foreach (StatBonus bonus in bonuses)
        {
            switch (bonus.modType)
            {
                case StatModType.Add:
                    fullValue += bonus.bonusValue;
                    break;
                case StatModType.Mult:
                    multiplier += bonus.bonusValue;
                    break;
                default:

                    break;
            }
        }

        return (int)(fullValue + fullValue * multiplier);
    }
}

[System.Serializable]
public class AbilityFlag
{
    public AbilityFlagType type;
    public bool value;
    [HideInInspector]
    public List<AbilityFlagBonus> bonuses;
    //Refence to the parent stats, mostly for secondary stats

    public AbilityFlag(AbilityFlagType t, bool startingValue = false)
    {
        type = t;
        value = startingValue;
        bonuses = new List<AbilityFlagBonus>();
    }

    public void AddBonus(AbilityFlagBonus bonus)
    {
        bonuses.Add(bonus);
    }

    public void RemoveBonus(AbilityFlagBonus bonus)
    {
        bonuses.Remove(bonus);
    }

    public bool GetBaseValue()
    {
        return value;
    }

    public virtual bool GetValue()
    {
        bool finalValue = value;

        foreach (AbilityFlagBonus bonus in bonuses)
        {
            if(!bonus.bonusValue)
            {
                return false;
            }
            else
            {
                finalValue = bonus.bonusValue;
            }
        }

        return finalValue;
    }
}

[System.Serializable]
public class AbilityFlagBonus
{
    public AbilityFlagType type;
    public bool bonusValue;

    public AbilityFlagBonus(AbilityFlagType t, bool bonus)
    {
        type = t;
        bonusValue = bonus;
    }

    public string GetTooltip()
    {
        string tooltip = "";

        return tooltip;
    }

}

[System.Serializable]
public class SecondaryStat
{
    public List<StatDependancy> statDependancies;
    public SecondaryStatType type;
    public Stats stats;
    public float value;
    [HideInInspector]
    public List<SecondaryStatBonus> bonuses;
    //Refence to the parent stats, mostly for secondary stats

    public void AddBonus(SecondaryStatBonus bonus)
    {
        bonuses.Add(bonus);
    }

    public void RemoveBonus(SecondaryStatBonus bonus)
    {
        bonuses.Remove(bonus);
    }

    public float GetBaseValue()
    {
        return value;
    }
    public SecondaryStat(SecondaryStatType t, Stats stats, List<StatDependancy> dependencies, int startingValue = 0)
    {
        this.stats = stats;
        statDependancies = dependencies;
        type = t;
        value = startingValue;
        bonuses = new List<SecondaryStatBonus>();
    }

    public float GetValue()
    {
        float fullValue = value;
        float multiplier = 0;

        foreach (StatDependancy dependency in statDependancies)
        {
            fullValue += stats.GetStat(dependency.type).GetValue() * dependency.valuePerStat;
        }

        foreach (SecondaryStatBonus bonus in bonuses)
        {
            switch (bonus.modType)
            {
                case StatModType.Add:
                    fullValue += bonus.bonusValue;
                    break;
                case StatModType.Mult:
                    multiplier += bonus.bonusValue;
                    break;
                default:

                    break;
            }
        }

        return (fullValue + fullValue * multiplier);

    }
}

public enum StatDependencyType { Flat, Percent }

[System.Serializable]
public class StatDependancy
{
    public StatType type;
    public float valuePerStat;

    public StatDependancy(StatType t, float min)
    {
        type = t;
        valuePerStat = min;
    }

}

//The multiplier mod type is really an additive multiplier. Adding a .5 mod adds 50%, it doesn't halve our value. That would be -.5
public enum StatModType { Add, Mult }

[System.Serializable]
public class StatBonus
{
    public StatModType modType = StatModType.Add;
    public StatType type;
    public float bonusValue;

    public StatBonus(StatType t, float min, StatModType modType = StatModType.Add)
    {
        type = t;
        bonusValue = min;
        this.modType = modType;
    }

    public string GetTooltip()
    {
        string tooltip = "";
        if (bonusValue >= 0)
        {
            tooltip += "+";
        }

        switch (modType)
        {
            case StatModType.Add:
                tooltip += bonusValue + " " + type.ToString();
                break;
            case StatModType.Mult:
                tooltip += bonusValue * 100 + "% " + type.ToString();
                break;
        }

        return tooltip;
    }
}

[System.Serializable]
public class SecondaryStatBonus
{
    public StatModType modType = StatModType.Add;
    public SecondaryStatType type;
    public float bonusValue;

    public SecondaryStatBonus(SecondaryStatType t, float min, StatModType modType = StatModType.Add)
    {
        type = t;
        bonusValue = min;
        this.modType = modType;
    }

    public string GetTooltip()
    {
        string tooltip = "";
        if (bonusValue >= 0)
        {
            tooltip += "+";
        } else
        {
            tooltip += "-";
        }

        switch (modType)
        {
            case StatModType.Add:
                tooltip += bonusValue + " " + type.ToString();
                break;
            case StatModType.Mult:
                tooltip += bonusValue + "% " + type.ToString();
                break;
        }

        return tooltip;
    }
}