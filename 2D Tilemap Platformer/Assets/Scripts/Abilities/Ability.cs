using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//These will be on items as bonuses with unique behaviours
public abstract class Ability : ScriptableObject
{
    protected Entity owner;

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
    }

    public virtual void OnAbilityLost()
    {

        owner.abilities.Remove(this);
        owner = null;
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

    }

    public virtual void OnHurt(AttackHitData hitData)
    {

    }

    //We may need killedEntity for some reason
    public virtual void OnKill(AttackHitData hitData)
    {

    }

    public virtual void OnJump()
    {

    }

    //This might need to be its own subAbility class
    public virtual void OnWalk()
    {

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
