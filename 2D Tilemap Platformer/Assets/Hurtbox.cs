using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hurtbox : MonoBehaviour
{
    private ColliderState colliderState = ColliderState.Open;
    public BoxCollider2D boxCollider;
    //The entity this hurtbox belongs to
    public Entity entity;

    //public Entity 
    public void GetHit(AttackObject attack)
    {
        Debug.Log(entity.name + " Hit by attack " + attack);
    }

}
