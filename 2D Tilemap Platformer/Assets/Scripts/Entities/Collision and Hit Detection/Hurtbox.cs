using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hurtbox : MonoBehaviour
{
    public ColliderState colliderState = ColliderState.Open;
    public BoxCollider2D boxCollider;
    //The entity this hurtbox belongs to
    public IHurtable owner;

    public void SetOwner(IHurtable newOwner)
    {
        owner = newOwner;
    }

    //public Entity 
    public void GetHurt(AttackObject attackObject)
    {
        if(owner == null)
        {
            return;
        }
        owner.GetHurt(attackObject);
    }

}
