using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//These will be on items as bonuses with unique behaviours
[CreateAssetMenu(fileName = "Ability", menuName = "ScriptableObjects/Abilities/Ability")]
public class Ability : ScriptableObject
{
    protected Entity owner;

    //do we need these?
    public List<StatBonus> bonusStats;
    public List<SecondaryStatBonus> secondaryBonusStats;
    public List<WeaponAttributeBonus> weaponBonuses;


    public Entity GetOwner()
    {
        return owner;
    }

    public void SetOwner(Entity entity)
    {
        owner = entity;
        owner.abilities.Add(this);
    }

    public virtual void GainAbility(Entity entity)
    {
        SetOwner(entity);
        owner.stats.AddPrimaryBonuses(bonusStats);
        owner.stats.AddSecondaryBonuses(secondaryBonusStats);
        owner.health.UpdateHealth();

        //This means talent cant be learned without a weapon... will have to update when weapon is equipped aswell
        if (owner is PlayerController player && player._equipmentManager.equippedWeapon != null)
        {
            player._equipmentManager.equippedWeapon.weaponAttributes.AddBonuses(weaponBonuses);

            player._attackManager.meleeWeaponObject.UpdateHitbox();
        }

    }

    public virtual void LoseAbility(Entity entity)
    {
        owner.abilities.Remove(this);
        owner.stats.RemovePrimaryBonuses(bonusStats);
        owner.stats.RemoveSecondaryBonuses(secondaryBonusStats);
        owner.health.UpdateHealth();

        if (owner is PlayerController player)
        {

            player._equipmentManager.equippedWeapon.weaponAttributes.RemoveBonuses(weaponBonuses);
            player._attackManager.meleeWeaponObject.UpdateHitbox();

        }
        owner = null;

    }


    public string GetTooltip()
    {
        string tooltip = "";

        tooltip += name.Replace("(Clone)", "");

        return tooltip;
    }
}
