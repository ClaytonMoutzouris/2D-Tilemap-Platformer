using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//These will be on items as bonuses with unique behaviours
[CreateAssetMenu(fileName = "OneTrueAbility", menuName = "ScriptableObjects/Abilities/OneTrueAbility")]
public class Ability : ScriptableObject
{
    protected Entity owner;
    public List<ItemRarity> itemRarities;
    public List<Effect> continuousEffects;
    //public List<Effect> OnGainedEffects;
    public List<Effect> OnHitGainedEffects;
    public List<Effect> OnHitInflictEffects;
    public List<Effect> OnHurtGainedEffects;
    public List<Effect> OnHurtInflictEffects;
    public List<Effect> OnKillEffects;
    public List<Effect> OnDieEffects;
    public List<Effect> OnWalkEffects;
    public List<Effect> OnJumpEffects;

    public Entity GetOwner()
    {
        return owner;
    }

    public void SetOwner(Entity entity)
    {
        owner = entity;
        owner.abilities.Add(this);
    }

    public bool CheckRarity(ItemRarity rarity)
    {
        return itemRarities.Contains(rarity);
    }

    public void InitEffects()
    {
        List<Effect> contEffects = new List<Effect>();

        foreach(Effect effect in continuousEffects)
        {
            contEffects.Add(Instantiate(effect));
        }

        continuousEffects.Clear();

        continuousEffects.AddRange(contEffects);
    }

    public virtual void OnGainedAbility(Entity entity)
    {
        SetOwner(entity);

        List<Effect> contEffects = new List<Effect>();

        foreach (Effect effect in continuousEffects)
        {
            contEffects.Add(Instantiate(effect));
        }

        continuousEffects.Clear();
        continuousEffects.AddRange(contEffects);

        //Apply all the continuous effects
        foreach (Effect effect in continuousEffects)
        {
            effect.ApplyEffect(owner);
        }
    }

    public virtual void OnAbilityLost()
    {

        owner.abilities.Remove(this);
        owner = null;

        foreach (Effect effect in continuousEffects)
        {
            effect.RemoveEffect();
        }
    }

    public virtual void OnEquippedWeapon(Weapon equipped)
    {
        foreach(Effect effect in continuousEffects)
        {
            effect.OnWeaponEquipped(equipped);
        }
    }

    public virtual void OnUnequippedWeapon(Weapon unequipped)
    {
        foreach (Effect effect in continuousEffects)
        {
            effect.OnWeaponUnequipped(unequipped);
        }
    }

    //Do I ever need to keep track of the exact effect that happens onHit?
    public virtual void OnHit(AttackHitData hitData)
    {
        foreach (Effect effect in OnHitGainedEffects)
        {
            Effect temp = Instantiate(effect);
            temp.ApplyEffect(owner, owner, hitData);
        }

        foreach (Effect effect in OnHitInflictEffects)
        {
            Effect temp = Instantiate(effect);
            temp.ApplyEffect(owner, hitData.hit.GetEntity(), hitData);
        }
    }

    public virtual void OnHurt(AttackHitData hitData)
    {
        foreach (Effect effect in OnHurtGainedEffects)
        {
            Effect temp = Instantiate(effect);
            temp.ApplyEffect(owner, owner, hitData);
        }

        foreach (Effect effect in OnHurtInflictEffects)
        {
            Effect temp = Instantiate(effect);
            temp.ApplyEffect(owner, hitData.attackOwner, hitData);
        }
    }

    //We may need killedEntity for some reason
    public virtual void OnKill(AttackHitData hitData)
    {
        foreach (Effect effect in OnKillEffects)
        {
            Effect temp = Instantiate(effect);
            temp.ApplyEffect(owner, hitData);
        }
    }

    public virtual void OnDeath(AttackHitData hitData)
    {
        foreach (Effect effect in OnDieEffects)
        {
            Effect temp = Instantiate(effect);
            temp.ApplyEffect(owner, hitData);
        }
    }

    public virtual void OnJump()
    {
        foreach (Effect effect in OnJumpEffects)
        {
            Effect temp = Instantiate(effect);
            temp.ApplyEffect(owner);
        }

    }

    //This might need to be its own subAbility class
    public virtual void OnWalk()
    {
        foreach (Effect effect in OnWalkEffects)
        {
            Effect temp = Instantiate(effect);
            temp.ApplyEffect(owner);
        }
    }

    public virtual string GetTooltip()
    {
        string tooltip = "";

        tooltip += name.Replace("(Clone)", "");

        return tooltip;
    }

    public virtual void RollAbility()
    {
        //Does nothing for most but is key for some
    }
}
