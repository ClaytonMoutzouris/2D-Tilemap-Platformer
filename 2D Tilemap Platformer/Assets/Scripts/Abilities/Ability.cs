using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//These will be on items as bonuses with unique behaviours
[CreateAssetMenu(fileName = "OneTrueAbility", menuName = "ScriptableObjects/Abilities/OneTrueAbility")]
public class Ability : ScriptableObject
{
    protected Entity owner;

    public List<Effect> continuousEffects;
    //public List<Effect> OnGainedEffects;
    public List<Effect> OnHitEffects;
    public List<Effect> OnHurtEffects;
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

    public virtual void OnGainedAbility(Entity entity)
    {
        SetOwner(entity);

        foreach(Effect effect in continuousEffects)
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

    }

    public virtual void OnUnequippedWeapon(Weapon unequipped)
    {

    }

    //Do I ever need to keep track of the exact effect that happens onHit?
    public virtual void OnHit(AttackHitData hitData)
    {
        foreach (Effect effect in OnHitEffects)
        {
            effect.ApplyEffect(owner, hitData);
        }
    }

    public virtual void OnHurt(AttackHitData hitData)
    {
        foreach (Effect effect in OnHurtEffects)
        {
            effect.ApplyEffect(owner, hitData);
        }
    }

    //We may need killedEntity for some reason
    public virtual void OnKill(AttackHitData hitData)
    {
        foreach (Effect effect in OnKillEffects)
        {
            effect.ApplyEffect(owner, hitData);
        }
    }

    public virtual void OnDeath(AttackHitData hitData)
    {
        foreach (Effect effect in OnDieEffects)
        {
            effect.ApplyEffect(owner, hitData);
        }
    }

    public virtual void OnJump()
    {
        foreach (Effect effect in OnJumpEffects)
        {
            effect.ApplyEffect(owner);
        }

    }

    //This might need to be its own subAbility class
    public virtual void OnWalk()
    {
        foreach (Effect effect in OnWalkEffects)
        {
            effect.ApplyEffect(owner);
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
