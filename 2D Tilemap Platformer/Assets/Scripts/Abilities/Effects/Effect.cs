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
    public bool stackable = false;
    [HideInInspector]
    public AttackHitData attackHitData;

    public virtual void ApplyEffect(Entity effected, AttackHitData data = null)
    {
        this.effectOwner = effected;
        effectedEntity = effected;
        attackHitData = data;
        //Run the coroutine on the entity
        //effectedEntity.StartCoroutine(HandleEffect());

        //overriding this should cover any instantaneous effects, right?
    }

    public virtual void ApplyEffect(Entity effected, Entity owner, AttackHitData data = null)
    {
        this.effectOwner = owner;
        effectedEntity = effected;
        attackHitData = data;
        //Run the coroutine on the entity
        //effectedEntity.StartCoroutine(HandleEffect());

        //overriding this should cover any instantaneous effects, right?
    }

    public virtual void RemoveEffect()
    {
        //remove this from the list of effects, if we added it?
    }
}
