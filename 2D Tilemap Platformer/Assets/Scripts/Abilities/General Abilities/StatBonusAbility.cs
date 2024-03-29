﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatBonusAbility", menuName = "ScriptableObjects/Abilities/StatBonusAbility")]
public class StatBonusAbility : Ability
{
    [Header("Stat Bonuses")]
    public List<StatBonus> bonusStats;
    public List<SecondaryStatBonus> secondaryBonusStats;
    //In the general version, these are applied to any equipped weapon
    public List<WeaponAttributeBonus> weaponBonuses;
    public List<AbilityFlagBonus> abilityFlagBonuses;

    public override void OnGainedAbility(Entity entity)
    {
        base.OnGainedAbility(entity);

        //Only update stats if the entity is a character.
        //Again, I might rework this
        if(owner is CharacterEntity character)
        {
            character.stats.AddPrimaryBonuses(bonusStats);
            character.stats.AddSecondaryBonuses(secondaryBonusStats);
            character.stats.AddAbilityFlagBonuses(abilityFlagBonuses);

            character.health.UpdateHealth();
        }

        if (owner is PlayerController player)
        {
            Weapon meleeEquipped = player._equipmentManager.GetEquippedWeapon(WeaponSlot.Melee);
            if (meleeEquipped != null)
            {
                meleeEquipped.weaponAttributes.AddBonuses(weaponBonuses);
                player._attackManager.meleeWeaponObject.UpdateHitbox();

            }

            Weapon rangedEquipped = player._equipmentManager.GetEquippedWeapon(WeaponSlot.Ranged);
            if (rangedEquipped != null)
            {
                rangedEquipped.weaponAttributes.AddBonuses(weaponBonuses);
                player._attackManager.rangedWeaponObject.UpdateHitbox();

            }

        }

    }

    public override void OnAbilityLost()
    {
        if (owner is CharacterEntity character)
        {
            character.stats.RemovePrimaryBonuses(bonusStats);
            character.stats.RemoveSecondaryBonuses(secondaryBonusStats);
            character.stats.RemoveAbilityFlagBonuses(abilityFlagBonuses);

            character.health.UpdateHealth();
        }

        if (owner is PlayerController player)
        {
            Weapon meleeEquipped = player._equipmentManager.GetEquippedWeapon(WeaponSlot.Melee);
            if (meleeEquipped != null)
            {
                meleeEquipped.weaponAttributes.RemoveBonuses(weaponBonuses);
                player._attackManager.meleeWeaponObject.UpdateHitbox();

            }

            Weapon rangedEquipped = player._equipmentManager.GetEquippedWeapon(WeaponSlot.Ranged);
            if (rangedEquipped != null)
            {
                rangedEquipped.weaponAttributes.RemoveBonuses(weaponBonuses);
                player._attackManager.rangedWeaponObject.UpdateHitbox();

            }

        }

        base.OnAbilityLost();

    }


    //These two make sure we apply and remove the weapon bonuses from any equipped weapon
    public override void OnEquippedWeapon(Weapon equipped)
    {
        base.OnEquippedWeapon(equipped);


        equipped.weaponAttributes.AddBonuses(weaponBonuses);

        if (owner is PlayerController player)
        {
            switch (equipped.weaponSlot)
            {
                case WeaponSlot.Melee:
                    player._attackManager.meleeWeaponObject.UpdateHitbox();
                    break;
                case WeaponSlot.Ranged:
                    player._attackManager.rangedWeaponObject.UpdateHitbox();
                    break;
                default:

                    break;
            }

        }

    }

    public override void OnUnequippedWeapon(Weapon unequipped)
    {
        base.OnUnequippedWeapon(unequipped);

        unequipped.weaponAttributes.RemoveBonuses(weaponBonuses);

        if (owner is PlayerController player)
        {
            switch(unequipped.weaponSlot)
            {
                case WeaponSlot.Melee:
                        player._attackManager.meleeWeaponObject.UpdateHitbox();
                    break;
                case WeaponSlot.Ranged:
                        player._attackManager.rangedWeaponObject.UpdateHitbox();
                    break;
                default:

                    break;
            }

        }

    }
}