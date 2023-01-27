using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Companion : Entity
{
    public int companionIndex = 0;
    public PlayerController owner;
    //public Attack attack; 
    //public Vector2 offset

    public void SetFromPrototype()
    {

    }

    public virtual void SetOwner(PlayerController player)
    {
        owner = player;
        player._companionManager.AddCompanion(this);
    }

    protected virtual void Update()
    {
        //what does a companion do?

        //maybe updating some abilities or some shit
    }

    public override bool CheckFriendly(Entity entity)
    {
        return (entity == this || entity == owner);
    }

}
