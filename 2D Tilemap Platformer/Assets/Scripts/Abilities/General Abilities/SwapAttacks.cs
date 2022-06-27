using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SwapAttacks", menuName = "ScriptableObjects/Abilities/SwapAttacks")]
public class SwapAttacks : Ability
{
    //This is a general version of the attack swaps, which will work for any weapon type
    //This causes some issues however, and this might be removed at some point
    public List<Attack> lightAttacks;
    public Attack heavyAttack;

    List<Attack> oldLightAttacks;
    Attack oldHeavyAttack;

    //This could potentially end up permanently changing the attacks if multiple swaps are made.

    public override void OnGainedAbility(Entity entity)
    {
        base.OnGainedAbility(entity);

        if (owner is PlayerController player && player._equipmentManager.equippedWeapon != null)
        {
            if(lightAttacks.Count > 0)
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

        if (owner is PlayerController player && player._equipmentManager.equippedWeapon != null)
        {
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

        if (owner is PlayerController player && player._equipmentManager.equippedWeapon != null)
        {
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

        if (owner is PlayerController player && player._equipmentManager.equippedWeapon != null)
        {
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