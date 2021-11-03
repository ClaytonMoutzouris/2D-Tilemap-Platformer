using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponAttributesType { Damage, DamageType, KnockbackPower, KnockbackAngle, CritChance, AttackSpeed, WeaponReach, FireRate, ProjectileSpeed, NumProjectiles, SpreadAngle, AmmoCapacity, ReloadTime, ProjectileLifeTime  };

[System.Serializable]
public class WeaponAttributes
{
    public Dictionary<WeaponAttributesType, WeaponAttribute> attributes;

    public WeaponAttributes()
    {

        attributes = new Dictionary<WeaponAttributesType, WeaponAttribute>();

        //Create starting stats
        foreach (WeaponAttributesType type in System.Enum.GetValues(typeof(WeaponAttributesType)))
        {
            attributes.Add(type, new WeaponAttribute(type));
        }

    }

    public void SetStartingStats(List<WeaponAttribute> attributesList)
    {
        if(attributes == null)
        {
            Debug.Log("Why are the stats null?");
            attributes = new Dictionary<WeaponAttributesType, WeaponAttribute>();
        }

        foreach (WeaponAttribute attribute in attributesList)
        {
            SetAttribute(attribute);
        }

    }

    public WeaponAttribute GetAttribute(WeaponAttributesType type)
    {
        if (attributes.ContainsKey(type)) {
            return attributes[type];
        }

        return null;
    }

    public void SetAttribute(WeaponAttribute value)
    {
        if (attributes.ContainsKey(value.type))
        {
            attributes[value.type] = value;
        }
        else
        {
            attributes.Add(value.type, value);
        }

    }

    public void AddBonus(WeaponAttributeBonus bonus)
    {
        if (attributes.ContainsKey(bonus.type))
        {
            attributes[bonus.type].AddBonus(bonus);
            //Debug.Log("Adding Weapon Bonus: " + bonus.type + " - " + bonus.bonusValue);
        }
        else
        {
            Debug.Log("Does not contain key " + bonus.type);

        }
    }

    public void RemoveBonus(WeaponAttributeBonus bonus)
    {
        if(bonus == null)
        {
            return;
        }

        if (attributes.ContainsKey(bonus.type))
        {
            attributes[bonus.type].RemoveBonus(bonus);
        }
    }

    public void AddBonuses(List<WeaponAttributeBonus> bonuses)
    {
        foreach (WeaponAttributeBonus bonus in bonuses)
        {
            AddBonus(bonus);
        }

    }

    public void RemoveBonuses(List<WeaponAttributeBonus> bonuses)
    {
        foreach (WeaponAttributeBonus bonus in bonuses)
        {
            RemoveBonus(bonus);
        }

    }
}

[System.Serializable]
public class WeaponAttribute
{
    public WeaponAttributesType type;
    public float value;
    [HideInInspector]
    public List<WeaponAttributeBonus> bonuses;

    public WeaponAttribute(WeaponAttributesType t, float startingValue = 0)
    {
        type = t;
        value = startingValue;
        bonuses = new List<WeaponAttributeBonus>();
    }

    public void AddBonus(WeaponAttributeBonus bonus)
    {
        bonuses.Add(bonus);
    }

    public void RemoveBonus(WeaponAttributeBonus bonus)
    {
        bonuses.Remove(bonus);
    }

    public float GetBaseValue()
    {
        return value;
    }

    public float GetValue()
    {
        float fullValue = value;
        float multiplier = 0;

        foreach (WeaponAttributeBonus bonus in bonuses)
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

[System.Serializable]
public class WeaponAttributeBonus
{

    //Weapon Class this bonus is for
    public StatModType modType;
    public WeaponAttributesType type;
    public float bonusValue;

    public WeaponAttributeBonus(WeaponAttributesType t, float min, StatModType modType = StatModType.Add)
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