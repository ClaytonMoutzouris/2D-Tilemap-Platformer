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

    public List<WeaponAttack> lightAttacks;
    public WeaponAttack heavyAttack;

    List<WeaponAttack> oldLightAttacks;
    WeaponAttack oldHeavyAttack;

    public override void OnGainedAbility(Entity entity)
    {
        base.OnGainedAbility(entity);

        if(owner is PlayerController player)
        {
            Weapon meleeEquipped = player._equipmentManager.GetEquippedWeapon(WeaponSlot.Melee);

            if(meleeEquipped != null && meleeEquipped.weaponClass == classType)
            {
                player.stats.AddPrimaryBonuses(bonusStats);
                player.stats.AddSecondaryBonuses(secondaryBonusStats);
                player.stats.AddAbilityFlagBonuses(abilityFlagBonuses);

                player.health.UpdateHealth();

                meleeEquipped.weaponAttributes.AddBonuses(weaponBonuses);
                player._attackManager.meleeWeaponObject.UpdateHitbox();

                if (lightAttacks.Count > 0)
                {
                    oldLightAttacks = meleeEquipped.attacks;
                    meleeEquipped.attacks = lightAttacks;

                }

                if (heavyAttack)
                {
                    oldHeavyAttack = meleeEquipped.heavyAttack;
                    meleeEquipped.heavyAttack = heavyAttack;
                }
            }

            Weapon rangedEquipped = player._equipmentManager.GetEquippedWeapon(WeaponSlot.Ranged);

            if (rangedEquipped != null && rangedEquipped.weaponClass == classType)
            {
                player.stats.AddPrimaryBonuses(bonusStats);
                player.stats.AddSecondaryBonuses(secondaryBonusStats);
                player.stats.AddAbilityFlagBonuses(abilityFlagBonuses);

                player.health.UpdateHealth();

                rangedEquipped.weaponAttributes.AddBonuses(weaponBonuses);
                player._attackManager.rangedWeaponObject.UpdateHitbox();

                if (lightAttacks.Count > 0)
                {
                    oldLightAttacks = rangedEquipped.attacks;
                    rangedEquipped.attacks = lightAttacks;

                }

                if (heavyAttack)
                {
                    oldHeavyAttack = rangedEquipped.heavyAttack;
                    rangedEquipped.heavyAttack = heavyAttack;
                }
            }
        }
    }

    public override void OnAbilityLost()
    {
        if(owner is PlayerController player)
        {
            Weapon meleeEquipped = player._equipmentManager.GetEquippedWeapon(WeaponSlot.Melee);

            if(meleeEquipped != null && meleeEquipped.weaponClass == classType)
            {
                player.stats.RemovePrimaryBonuses(bonusStats);
                player.stats.RemoveSecondaryBonuses(secondaryBonusStats);
                player.stats.RemoveAbilityFlagBonuses(abilityFlagBonuses);

                player.health.UpdateHealth();

                meleeEquipped.weaponAttributes.RemoveBonuses(weaponBonuses);
                player._attackManager.meleeWeaponObject.UpdateHitbox();

                if (lightAttacks.Count > 0)
                {
                    meleeEquipped.attacks = oldLightAttacks;
                    oldLightAttacks.Clear();

                }

                if (heavyAttack)
                {
                    meleeEquipped.heavyAttack = oldHeavyAttack;
                    oldHeavyAttack = null;
                }
            }

            Weapon rangedEquipped = player._equipmentManager.GetEquippedWeapon(WeaponSlot.Ranged);

            if (rangedEquipped != null && rangedEquipped.weaponClass == classType)
            {
                player.stats.RemovePrimaryBonuses(bonusStats);
                player.stats.RemoveSecondaryBonuses(secondaryBonusStats);
                player.stats.RemoveAbilityFlagBonuses(abilityFlagBonuses);

                player.health.UpdateHealth();

                rangedEquipped.weaponAttributes.RemoveBonuses(weaponBonuses);
                player._attackManager.rangedWeaponObject.UpdateHitbox();

                if (lightAttacks.Count > 0)
                {
                    rangedEquipped.attacks = oldLightAttacks;
                    oldLightAttacks.Clear();

                }

                if (heavyAttack)
                {
                    rangedEquipped.heavyAttack = oldHeavyAttack;
                    oldHeavyAttack = null;
                }
            }
        }

        base.OnAbilityLost();

    }


    //These two make sure we apply and remove the weapon bonuses from any equipped weapon
    public override void OnEquippedWeapon(Weapon equipped)
    {
        base.OnEquippedWeapon(equipped);

        if (owner is PlayerController player && equipped.weaponClass == classType)
        {
            player.stats.AddPrimaryBonuses(bonusStats);
            player.stats.AddSecondaryBonuses(secondaryBonusStats);
            player.stats.AddAbilityFlagBonuses(abilityFlagBonuses);

            player.health.UpdateHealth();

            equipped.weaponAttributes.AddBonuses(weaponBonuses);
            player._attackManager.meleeWeaponObject.UpdateHitbox();

            if (lightAttacks.Count > 0)
            {
                oldLightAttacks = equipped.attacks;
                equipped.attacks = lightAttacks;

            }

            if (heavyAttack)
            {
                oldHeavyAttack = equipped.heavyAttack;
                equipped.heavyAttack = heavyAttack;
            }
        }

    }

    public override void OnUnequippedWeapon(Weapon unequipped)
    {
        base.OnUnequippedWeapon(unequipped);

        if (owner is PlayerController player && unequipped.weaponClass == classType)
        {
            player.stats.RemovePrimaryBonuses(bonusStats);
            player.stats.RemoveSecondaryBonuses(secondaryBonusStats);
            player.stats.RemoveAbilityFlagBonuses(abilityFlagBonuses);

            player.health.UpdateHealth();

            unequipped.weaponAttributes.RemoveBonuses(weaponBonuses);
            player._attackManager.meleeWeaponObject.UpdateHitbox();

            if (lightAttacks.Count > 0)
            {
                unequipped.attacks = oldLightAttacks;
                oldLightAttacks.Clear();

            }

            if (heavyAttack)
            {
                unequipped.heavyAttack = oldHeavyAttack;
                oldHeavyAttack = null;
            }
        }

    }
}