using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * A class for abilities that buff only certain weapon types
 */
[CreateAssetMenu(fileName = "WeaponAbility", menuName = "ScriptableObjects/Abilities/WeaponAbility")]
public class WeaponAbility : Ability
{

    public WeaponClassType weaponClass; 
    public Attack lightAttack;
    public Attack heavyAttack;

    [HideInInspector]
    public Attack oldLightAttack;
    [HideInInspector]
    public Attack oldHeavyAttack;

    public List<ProjectileFlagBonus> projectileEffects;
    public List<StatBonus> wepBonusStats;
    public List<SecondaryStatBonus> wepSecondaryBonusStats;
    public List<WeaponAttributeBonus> wepWeaponBonuses;

    
    public override void GainAbility(Entity entity)
    {
        base.GainAbility(entity);

        Debug.Log("Weapon ability");
        Debug.Log("Owner " + owner);
        Debug.Log("Owner " + owner);

        OnWeaponEquipped();


        //TODO: This

    }

    public override void LoseAbility(Entity entity)
    {
        OnWeaponUnequipped();

        base.LoseAbility(entity);


    }

    public void OnWeaponEquipped()
    {
        if (owner is PlayerController player && player._equipmentManager.equippedWeapon != null && player._equipmentManager.equippedWeapon.weaponClass == weaponClass)
        {

            owner.stats.AddPrimaryBonuses(wepBonusStats);
            owner.stats.AddSecondaryBonuses(wepSecondaryBonusStats);
            owner.health.UpdateHealth();
            player._equipmentManager.equippedWeapon.weaponAttributes.AddBonuses(weaponBonuses);

            //Instead of changing the projectiles data, the weapon should hold a verion of this... right?
            player._equipmentManager.equippedWeapon.projectileBonuses.AddRange(projectileEffects);

            player._attackManager.meleeWeaponObject.UpdateHitbox();

            if (lightAttack != null)
            {
                oldLightAttack = player._equipmentManager.equippedWeapon.attacks[0];
                player._equipmentManager.equippedWeapon.attacks[0] = lightAttack;
            }

            if (heavyAttack != null)
            {
                oldHeavyAttack = player._equipmentManager.equippedWeapon.heavyAttack;
                player._equipmentManager.equippedWeapon.heavyAttack = heavyAttack;
            }

        }
    }

    public void OnWeaponUnequipped()
    {
        if (owner is PlayerController player && player._equipmentManager.equippedWeapon != null && player._equipmentManager.equippedWeapon.weaponClass == weaponClass)
        {
            owner.stats.RemovePrimaryBonuses(wepBonusStats);
            owner.stats.RemoveSecondaryBonuses(wepSecondaryBonusStats);
            owner.health.UpdateHealth();
            player._equipmentManager.equippedWeapon.weaponAttributes.RemoveBonuses(weaponBonuses);

            //No easier way to do this
            foreach(ProjectileFlagBonus bonus in projectileEffects)
            {
                player._equipmentManager.equippedWeapon.projectileBonuses.Remove(bonus);
            }

            player._attackManager.meleeWeaponObject.UpdateHitbox();

            if (lightAttack != null)
            {
                player._equipmentManager.equippedWeapon.attacks[0] = oldLightAttack;
            }

            if (heavyAttack != null)
            {
                player._equipmentManager.equippedWeapon.heavyAttack = oldHeavyAttack;
            }
        }
    }
    
}
