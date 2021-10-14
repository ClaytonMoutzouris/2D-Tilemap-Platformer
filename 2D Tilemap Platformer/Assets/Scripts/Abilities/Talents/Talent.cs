using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//These will be on items as bonuses with unique behaviours
[CreateAssetMenu(fileName = "Talent", menuName = "ScriptableObjects/Talents/Talent")]
public class Talent : ScriptableObject
{
    protected PlayerController owner;
    //do we need these?
    public List<StatBonus> bonusStats;
    public List<SecondaryStatBonus> secondaryBonusStats;
    public List<WeaponAttributeBonus> weaponBonuses;

    public List<Ability> abilities;
    public bool isLearned = false;


    public PlayerController GetOwner()
    {
        return owner;
    }

    public void SetOwner(PlayerController player)
    {
        owner = player;
        //owner.abilities.Add(this);
    }

    public void LearnTalent(PlayerController player)
    {
        if(isLearned)
        {
            return;
        }
        SetOwner(player);
        isLearned = true;
        owner = player;
        owner.stats.AddPrimaryBonuses(bonusStats);
        owner.stats.AddSecondaryBonuses(secondaryBonusStats);

        List<Ability> temp = new List<Ability>();

        foreach (Ability ability in abilities)
        {
            temp.Add(Instantiate(ability));
        }

        abilities = temp;

        foreach (Ability ability in abilities)
        {
            ability.GainAbility(owner);
        }

        owner.health.UpdateHealth();

        if (player._equipmentManager.equippedWeapon != null)
        {
            player._equipmentManager.equippedWeapon.weaponAttributes.AddBonuses(weaponBonuses);
            player._attackManager.meleeWeaponObject.UpdateHitbox();
        }

        owner.learnedTalents.Add(this);

    }

    public void OnEquipWeapon()
    {

    }

    public void UnlearnTalent()
    {
        if(owner == null || !isLearned)
        {
            return;
        }
        isLearned = false;
        //owner.abilities.Remove(this);
        owner.stats.RemovePrimaryBonuses(bonusStats);
        owner.stats.RemoveSecondaryBonuses(secondaryBonusStats);

        foreach (Ability ability in abilities)
        {
            ability.LoseAbility(owner);
        }

        owner.health.UpdateHealth();

        if (owner._equipmentManager.equippedWeapon != null)
        {
            owner._equipmentManager.equippedWeapon.weaponAttributes.RemoveBonuses(weaponBonuses);
            owner._attackManager.meleeWeaponObject.UpdateHitbox();

        }
        owner = null;

        owner.learnedTalents.Remove(this);

    }

    public string GetTooltip()
    {
        string tooltip = "";

        tooltip += "<color=white>" + name.Replace("(Clone)", "") + "</color>";

        foreach(StatBonus bonus in bonusStats)
        {
            tooltip += "\n<color=yellow>" + bonus.GetTooltip() + "</color>";
        }


        foreach (SecondaryStatBonus bonus in secondaryBonusStats)
        {
            tooltip += "\n<color=yellow>" + bonus.GetTooltip() + "</color>\n";
        }

        foreach (WeaponAttributeBonus bonus in weaponBonuses)
        {
            tooltip += "\n<color=yellow>" + bonus.GetTooltip() + "</color>\n";
        }

        foreach (Ability ability in abilities)
        {
            tooltip += "\n<color=green>" + ability.GetTooltip() + "</color>\n";
        }



        return tooltip;
    }
}
