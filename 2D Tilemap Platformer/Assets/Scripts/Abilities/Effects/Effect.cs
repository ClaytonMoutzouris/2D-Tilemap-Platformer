using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : ScriptableObject
{
    [HideInInspector]
    public Entity effectOwner;
    [HideInInspector]
    public Entity effectedEntity;
    //Might not need this one, but will keep it around for now
    [HideInInspector]
    public AttackHitData attackHitData;

    public virtual void ApplyEffect(Entity owner, Entity effected, AttackHitData data = null)
    {

        this.effectOwner = owner;
        effectedEntity = effected;
        attackHitData = data;
        //Run the coroutine on the entity
        //effectedEntity.StartCoroutine(HandleEffect());

        //overriding this should cover any instantaneous effects, right?
    }

    public virtual bool CheckRequirements(Entity owner, Entity effected)
    {
        bool canApply = true;

        return canApply;

    }



    //For personal effects
    public virtual void ApplyEffect(Entity owner)
    {
        ApplyEffect(owner, owner, null);
    }

    //For attack effects
    public virtual void ApplyEffect(Entity owner, AttackHitData data)
    {
        ApplyEffect(owner, owner, data);
    }

    public virtual void RemoveEffect()
    {
        //remove this from the list of effects, if we added it?
    }

    public virtual void OnWeaponEquipped(Weapon wep)
    {

    }

    public virtual void OnWeaponUnequipped(Weapon wep)
    {

    }

}
