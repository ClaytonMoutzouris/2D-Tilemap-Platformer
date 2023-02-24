using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatBonusEffect", menuName = "ScriptableObjects/Effects/StatBonusEffect")]
public class StatBonusEffect : Effect
{
    public List<StatBonus> statBonuses;
    public List<SecondaryStatBonus> secondaryBonusStats;
    public List<AbilityFlagBonus> abilityFlagBonuses;
    //This is global to all weapons equipped, all types and both slot types
    public List<WeaponAttributeBonus> weaponBonuses;


    public override void ApplyEffect(Entity owner, Entity effected, AttackHitData data = null)
    {
        base.ApplyEffect(owner, effected, data);
        //Run the coroutine on the entity

        //effectedEntity.StartCoroutine(HandleEffect());
        if (effectedEntity is CharacterEntity character)
        {
            character.stats.AddPrimaryBonuses(statBonuses);
            character.stats.AddSecondaryBonuses(secondaryBonusStats);
            character.stats.AddAbilityFlagBonuses(abilityFlagBonuses);
            
            character.health.UpdateHealth();
            Debug.Log("Players new max health " + character.health.maxHealth);
        }

        if(effectedEntity is PlayerController player)
        {
            player._equipmentManager.GetEquippedWeapon(WeaponSlot.Melee).weaponAttributes.AddBonuses(weaponBonuses);
            player._equipmentManager.GetEquippedWeapon(WeaponSlot.Melee).UpdateAmmoCapacity();
        }



        //overriding this should cover any instantaneous effects, right?
    }

    public override void RemoveEffect()
    {
        if (effectedEntity is CharacterEntity character)
        {
            character.stats.RemovePrimaryBonuses(statBonuses);
            character.stats.RemoveSecondaryBonuses(secondaryBonusStats);
            character.stats.RemoveAbilityFlagBonuses(abilityFlagBonuses);

            character.health.UpdateHealth();
        }

        if (effectedEntity is PlayerController player)
        {
            player._equipmentManager.GetEquippedWeapon(WeaponSlot.Melee).weaponAttributes.RemoveBonuses(weaponBonuses);
            player._equipmentManager.GetEquippedWeapon(WeaponSlot.Melee).UpdateAmmoCapacity();

            player._equipmentManager.GetEquippedWeapon(WeaponSlot.Ranged).weaponAttributes.RemoveBonuses(weaponBonuses);
            player._equipmentManager.GetEquippedWeapon(WeaponSlot.Ranged).UpdateAmmoCapacity();
        }

        base.RemoveEffect();
        //remove this from the list of effects, if we added it?
    }

    public override void OnWeaponEquipped(Weapon wep)
    {
        base.OnWeaponEquipped(wep);

        if (wep != null)
        {
            wep.weaponAttributes.AddBonuses(weaponBonuses);
            wep.UpdateAmmoCapacity();
        }
    }

    public override void OnWeaponUnequipped(Weapon wep)
    {
        if (wep != null)
        {
            wep.weaponAttributes.RemoveBonuses(weaponBonuses);
            wep.UpdateAmmoCapacity();
        }
    }

}
