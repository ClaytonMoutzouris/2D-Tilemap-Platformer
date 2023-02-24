using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponBonusEffect", menuName = "ScriptableObjects/Effects/WeaponBonusEffect")]
public class WeaponBonusEffect : Effect
{
    public bool restrictWeaponType = false;
    public WeaponClassType weaponTypeRestriction;
    public WeaponSlot weaponSlot;

    public List<StatBonus> statBonuses;
    public List<SecondaryStatBonus> secondaryBonusStats;
    public List<AbilityFlagBonus> abilityFlagBonuses;
    //This is global to all weapons equipped, all types and both slot types
    public List<WeaponAttributeBonus> weaponBonuses;

    public List<Effect> weaponHitGainEffects;
    public List<Effect> weaponHitInflictEffects;
    public List<Effect> weaponAttackGainEffects;
    public List<Effect> weaponAttackInflictEffects;
    public List<Effect> weaponKillEffects;
    public List<Effect> projectileDestroyEffects;

    public override void ApplyEffect(Entity owner, Entity effected, AttackHitData data = null)
    {
        base.ApplyEffect(owner, effected, data);

        if (effectedEntity is PlayerController player)
        {
            Weapon weapon = player._equipmentManager.GetEquippedWeapon(weaponSlot);
            if (!effectedEntity.weaponEffects.Contains(this) && weapon != null && (!restrictWeaponType || weaponTypeRestriction == weapon.weaponClass))
            {
                player.stats.AddPrimaryBonuses(statBonuses);
                player.stats.AddSecondaryBonuses(secondaryBonusStats);
                player.stats.AddAbilityFlagBonuses(abilityFlagBonuses);

                player.health.UpdateHealth();

                weapon.weaponAttributes.AddBonuses(weaponBonuses);

                foreach(Effect effect in weaponHitGainEffects)
                {
                    weapon.OnHitGainEffects.Add(effect);
                }

                foreach (Effect effect in weaponAttackInflictEffects)
                {
                    weapon.OnAttackInflictEffects.Add(effect);
                }

                foreach (Effect effect in weaponKillEffects)
                {
                    weapon.OnKillEffects.Add(effect);
                }

                foreach (Effect effect in projectileDestroyEffects)
                {
                    weapon.OnDestroyEffects.Add(effect);
                }

                foreach (Effect effect in weaponHitInflictEffects)
                {
                    weapon.OnHitInflictEffects.Add(effect);
                }

                foreach (Effect effect in weaponAttackGainEffects)
                {
                    weapon.OnAttackGainEffects.Add(effect);
                }

                effectedEntity.weaponEffects.Add(this);
                
            }
        }
    }



    public override void RemoveEffect()
    {
        if (effectedEntity is PlayerController player)
        {
            Weapon weapon = player._equipmentManager.GetEquippedWeapon(weaponSlot);
            if (effectedEntity.weaponEffects.Contains(this) && weapon != null && (!restrictWeaponType || weaponTypeRestriction == weapon.weaponClass))
            {
                player.stats.RemovePrimaryBonuses(statBonuses);
                player.stats.RemoveSecondaryBonuses(secondaryBonusStats);
                player.stats.RemoveAbilityFlagBonuses(abilityFlagBonuses);

                player.health.UpdateHealth();

                weapon.weaponAttributes.RemoveBonuses(weaponBonuses);

                foreach (Effect effect in weaponHitGainEffects)
                {
                    weapon.OnHitGainEffects.Remove(effect);
                }

                foreach (Effect effect in weaponAttackInflictEffects)
                {
                    weapon.OnAttackInflictEffects.Remove(effect);
                }

                foreach (Effect effect in weaponKillEffects)
                {
                    weapon.OnKillEffects.Remove(effect);
                }

                foreach (Effect effect in projectileDestroyEffects)
                {
                    weapon.OnDestroyEffects.Remove(effect);
                }

                
                foreach (Effect effect in weaponHitInflictEffects)
                {
                    weapon.OnHitInflictEffects.Remove(effect);
                }

                foreach (Effect effect in weaponAttackGainEffects)
                {
                    weapon.OnAttackGainEffects.Remove(effect);
                }

                effectedEntity.weaponEffects.Remove(this);

            }
        }


        base.RemoveEffect();
    }

    public override void OnWeaponEquipped(Weapon wep)
    {
        if (!effectedEntity.weaponEffects.Contains(this) && wep != null && wep.weaponSlot == weaponSlot && (!restrictWeaponType || weaponTypeRestriction == wep.weaponClass))
        {
            if(effectedEntity is PlayerController player)
            {
                player.stats.AddPrimaryBonuses(statBonuses);
                player.stats.AddSecondaryBonuses(secondaryBonusStats);
                player.stats.AddAbilityFlagBonuses(abilityFlagBonuses);

                player.health.UpdateHealth();
            }

            wep.weaponAttributes.AddBonuses(weaponBonuses);
            foreach (Effect effect in weaponHitGainEffects)
            {
                wep.OnHitGainEffects.Add(effect);
            }

            foreach (Effect effect in weaponAttackInflictEffects)
            {
                wep.OnAttackInflictEffects.Add(effect);
            }

            foreach (Effect effect in weaponKillEffects)
            {
                wep.OnKillEffects.Add(effect);
            }

            foreach (Effect effect in projectileDestroyEffects)
            {
                wep.OnDestroyEffects.Add(effect);
            }


            foreach (Effect effect in weaponHitInflictEffects)
            {
                wep.OnHitInflictEffects.Add(effect);
            }

            foreach (Effect effect in weaponAttackGainEffects)
            {
                wep.OnAttackGainEffects.Add(effect);
            }

            effectedEntity.weaponEffects.Add(this);
            
        }
    }

    public override void OnWeaponUnequipped(Weapon wep)
    {
        if (effectedEntity.weaponEffects.Contains(this) && wep != null && wep.weaponSlot == weaponSlot && (!restrictWeaponType || weaponTypeRestriction == wep.weaponClass))
        {
            if (effectedEntity is PlayerController player)
            {
                player.stats.RemovePrimaryBonuses(statBonuses);
                player.stats.RemoveSecondaryBonuses(secondaryBonusStats);
                player.stats.RemoveAbilityFlagBonuses(abilityFlagBonuses);

                player.health.UpdateHealth();
            }

            wep.weaponAttributes.RemoveBonuses(weaponBonuses);

            foreach (Effect effect in weaponHitGainEffects)
            {
                wep.OnHitGainEffects.Remove(effect);
            }

            foreach (Effect effect in weaponAttackInflictEffects)
            {
                wep.OnAttackInflictEffects.Remove(effect);
            }

            foreach (Effect effect in weaponKillEffects)
            {
                wep.OnKillEffects.Remove(effect);
            }

            foreach (Effect effect in projectileDestroyEffects)
            {
                wep.OnDestroyEffects.Remove(effect);
            }


            foreach (Effect effect in weaponHitInflictEffects)
            {
                wep.OnHitInflictEffects.Remove(effect);
            }

            foreach (Effect effect in weaponAttackGainEffects)
            {
                wep.OnAttackGainEffects.Remove(effect);
            }

            effectedEntity.weaponEffects.Remove(this);
        }
    }
}
