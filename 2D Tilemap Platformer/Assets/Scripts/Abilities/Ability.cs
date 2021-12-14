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
    public List<AbilityFlagBonus> abilityFlagBonuses;

    //Effects that should be applied and removed as the ability is gained/removed
    public List<Effect> OnGainedEffects;

    //Effects that are applied as a new weapon is equipped/unequipped
    public List<Effect> OnEquippedEffects;

    public List<Effect> OnHitEffects;

    public List<Effect> OnHurtEffects;

    public List<Effect> OnJumpEffects;

    public List<Effect> OnKillEffects;

    public List<Effect> OnWalkEffects;

    public Entity GetOwner()
    {
        return owner;
    }

    public void SetOwner(Entity entity)
    {
        owner = entity;
        owner.abilities.Add(this);
    }

    public virtual void OnGainedAbility(Entity entity)
    {
        foreach(Effect effect in OnGainedEffects)
        {
            //effect.Trigger(entity);
            //Apply all these effects to the entity
        }
    }

    public virtual void OnAbilityLost(Entity entity)
    {
        foreach (Effect effect in OnGainedEffects)
        {
            //effect.Trigger(entity);
            //Remove all these effects from the entity
        }
    }

    public virtual void OnEquippedWeapon(Entity entity)
    {
        foreach (Effect effect in OnEquippedEffects)
        {
            //effect.Trigger(entity);
            //Apply all these effects to the entity
        }
    }

    public virtual void OnUnequippedWeapon(Entity entity)
    {
        foreach (Effect effect in OnEquippedEffects)
        {
            //effect.Trigger(entity);
            //Remove all these effects from the entity
        }
    }

    public virtual void GainAbility(Entity entity)
    {
        SetOwner(entity);
        owner.stats.AddPrimaryBonuses(bonusStats);
        owner.stats.AddSecondaryBonuses(secondaryBonusStats);
        owner.stats.AddAbilityFlagBonuses(abilityFlagBonuses);

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
        owner.stats.RemoveAbilityFlagBonuses(abilityFlagBonuses);

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
