using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponSpecialization", menuName = "ScriptableObjects/Abilities/WeaponSpecialization")]
public class WeaponSpecialization : Ability
{
    //These bonuses will only be applied if the player has a weapon equipped of the specified type
    public WeaponClassType classType;
    public List<StatBonus> bonusStats;
    public List<SecondaryStatBonus> secondaryBonusStats;
    public List<WeaponAttributeBonus> weaponBonuses;
    public List<AbilityFlagBonus> abilityFlagBonuses;

    public List<Attack> lightAttacks;
    public Attack heavyAttack;

    List<Attack> oldLightAttacks;
    Attack oldHeavyAttack;

    public override void OnGainedAbility(Entity entity)
    {
        base.OnGainedAbility(entity);

        if (owner is PlayerController player && player._equipmentManager.equippedWeapon != null && player._equipmentManager.equippedWeapon.weaponClass == classType)
        {

            player.stats.AddPrimaryBonuses(bonusStats);
            player.stats.AddSecondaryBonuses(secondaryBonusStats);
            player.stats.AddAbilityFlagBonuses(abilityFlagBonuses);

            player.health.UpdateHealth();

            player._equipmentManager.equippedWeapon.weaponAttributes.AddBonuses(weaponBonuses);
            player._attackManager.meleeWeaponObject.UpdateHitbox();

            if (lightAttacks.Count > 0)
            {
                oldLightAttacks = player._equipmentManager.equippedWeapon.attacks;
                player._equipmentManager.equippedWeapon.attacks = lightAttacks;

            }

            if (heavyAttack)
            {
                oldHeavyAttack = player._equipmentManager.equippedWeapon.heavyAttack;
                player._equipmentManager.equippedWeapon.heavyAttack = heavyAttack;
            }
        }
    }

    public override void OnAbilityLost()
    {
        if (owner is PlayerController player && player._equipmentManager.equippedWeapon != null && player._equipmentManager.equippedWeapon.weaponClass == classType)
        {
            player.stats.RemovePrimaryBonuses(bonusStats);
            player.stats.RemoveSecondaryBonuses(secondaryBonusStats);
            player.stats.RemoveAbilityFlagBonuses(abilityFlagBonuses);

            player.health.UpdateHealth();

            player._equipmentManager.equippedWeapon.weaponAttributes.RemoveBonuses(weaponBonuses);
            player._attackManager.meleeWeaponObject.UpdateHitbox();

            if (lightAttacks.Count > 0)
            {
                player._equipmentManager.equippedWeapon.attacks = oldLightAttacks;
                oldLightAttacks.Clear();

            }

            if (heavyAttack)
            {
                player._equipmentManager.equippedWeapon.heavyAttack = oldHeavyAttack;
                oldHeavyAttack = null;
            }
        }

        base.OnAbilityLost();

    }


    //These two make sure we apply and remove the weapon bonuses from any equipped weapon
    public override void OnEquippedWeapon()
    {
        base.OnEquippedWeapon();

        if (owner is PlayerController player && player._equipmentManager.equippedWeapon != null && player._equipmentManager.equippedWeapon.weaponClass == classType)
        {
            player.stats.AddPrimaryBonuses(bonusStats);
            player.stats.AddSecondaryBonuses(secondaryBonusStats);
            player.stats.AddAbilityFlagBonuses(abilityFlagBonuses);

            player.health.UpdateHealth();

            player._equipmentManager.equippedWeapon.weaponAttributes.AddBonuses(weaponBonuses);
            player._attackManager.meleeWeaponObject.UpdateHitbox();

            if (lightAttacks.Count > 0)
            {
                oldLightAttacks = player._equipmentManager.equippedWeapon.attacks;
                player._equipmentManager.equippedWeapon.attacks = lightAttacks;

            }

            if (heavyAttack)
            {
                oldHeavyAttack = player._equipmentManager.equippedWeapon.heavyAttack;
                player._equipmentManager.equippedWeapon.heavyAttack = heavyAttack;
            }
        }

    }

    public override void OnUnequippedWeapon()
    {
        base.OnUnequippedWeapon();

        if (owner is PlayerController player && player._equipmentManager.equippedWeapon != null && player._equipmentManager.equippedWeapon.weaponClass == classType)
        {
            player.stats.RemovePrimaryBonuses(bonusStats);
            player.stats.RemoveSecondaryBonuses(secondaryBonusStats);
            player.stats.RemoveAbilityFlagBonuses(abilityFlagBonuses);

            player.health.UpdateHealth();

            player._equipmentManager.equippedWeapon.weaponAttributes.RemoveBonuses(weaponBonuses);
            player._attackManager.meleeWeaponObject.UpdateHitbox();

            if (lightAttacks.Count > 0)
            {
                player._equipmentManager.equippedWeapon.attacks = oldLightAttacks;
                oldLightAttacks.Clear();

            }

            if (heavyAttack)
            {
                player._equipmentManager.equippedWeapon.heavyAttack = oldHeavyAttack;
                oldHeavyAttack = null;
            }
        }

    }
}