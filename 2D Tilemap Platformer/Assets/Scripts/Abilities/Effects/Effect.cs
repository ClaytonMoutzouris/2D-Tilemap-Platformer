using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : ScriptableObject
{
    [HideInInspector]
    public Entity effected;
    //Might not need this one, but will keep it around for now
    [HideInInspector]
    public Entity effectOwner;
    public bool stackable = false;


    public virtual void ApplyEffect(Entity effected, Entity owner = null)
    {
        this.effected = effected;
        effectOwner = owner;
        //Run the coroutine on the entity
        //effectedEntity.StartCoroutine(HandleEffect());
    }

}
