using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SwapAttacks", menuName = "ScriptableObjects/Abilities/SwapAttacks")]
public class SwapAttacks : Ability
{
    //This is a general version of the attack swaps, which will work for any weapon type
    //This causes some issues however, and this might be removed at some point
    public List<WeaponAttack> lightAttacks;
    public WeaponAttack heavyAttack;

    List<WeaponAttack> oldLightAttacks;
    WeaponAttack oldHeavyAttack;

    //This could potentially end up permanently changing the attacks if multiple swaps are made.

    public override void OnGainedAbility(Entity entity)
    {
        base.OnGainedAbility(entity);


        if (owner is PlayerController player)
        {
            Weapon meleeEquipped = player._equipmentManager.GetEquippedWeapon(WeaponSlot.Melee);
            if (meleeEquipped != null)
            {
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
            if (rangedEquipped != null)
            {
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

        if (owner is PlayerController player)
        {
            Weapon meleeEquipped = player._equipmentManager.GetEquippedWeapon(WeaponSlot.Melee);
            if (meleeEquipped != null)
            {
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
            if (rangedEquipped != null)
            {
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

    public override void OnUnequippedWeapon(Weapon unequipped)
    {
        base.OnUnequippedWeapon(unequipped);

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