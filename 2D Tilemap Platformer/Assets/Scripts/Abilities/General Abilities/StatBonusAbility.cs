using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatBonusAbility", menuName = "ScriptableObjects/Abilities/StatBonusAbility")]
public class StatBonusAbility : Ability
{
    public List<StatBonus> bonusStats;
    public List<SecondaryStatBonus> secondaryBonusStats;
    //In the general version, these are applied to any equipped weapon
    public List<WeaponAttributeBonus> weaponBonuses;
    public List<AbilityFlagBonus> abilityFlagBonuses;

    public override void OnGainedAbility(Entity entity)
    {
        base.OnGainedAbility(entity);

        owner.stats.AddPrimaryBonuses(bonusStats);
        owner.stats.AddSecondaryBonuses(secondaryBonusStats);
        owner.stats.AddAbilityFlagBonuses(abilityFlagBonuses);

        owner.health.UpdateHealth();

        if (owner is PlayerController player && player._equipmentManager.equippedWeapon != null)
        {
            player._equipmentManager.equippedWeapon.weaponAttributes.AddBonuses(weaponBonuses);
            player._attackManager.meleeWeaponObject.UpdateHitbox();
        }
    }

    public override void OnAbilityLost()
    {

        owner.stats.RemovePrimaryBonuses(bonusStats);
        owner.stats.RemoveSecondaryBonuses(secondaryBonusStats);
        owner.stats.RemoveAbilityFlagBonuses(abilityFlagBonuses);

        owner.health.UpdateHealth();

        if (owner is PlayerController player && player._equipmentManager.equippedWeapon != null)
        {
            player._equipmentManager.equippedWeapon.weaponAttributes.RemoveBonuses(weaponBonuses);
            player._attackManager.meleeWeaponObject.UpdateHitbox();

        }

        base.OnAbilityLost();

    }


    //These two make sure we apply and remove the weapon bonuses from any equipped weapon
    public override void OnEquippedWeapon()
    {
        base.OnEquippedWeapon();

        if (owner is PlayerController player && player._equipmentManager.equippedWeapon != null)
        {
            player._equipmentManager.equippedWeapon.weaponAttributes.AddBonuses(weaponBonuses);
            player._attackManager.meleeWeaponObject.UpdateHitbox();
        }

    }

    public override void OnUnequippedWeapon()
    {
        base.OnUnequippedWeapon();

        if (owner is PlayerController player && player._equipmentManager.equippedWeapon != null)
        {
            player._equipmentManager.equippedWeapon.weaponAttributes.RemoveBonuses(weaponBonuses);
            player._attackManager.meleeWeaponObject.UpdateHitbox();
        }

    }
}