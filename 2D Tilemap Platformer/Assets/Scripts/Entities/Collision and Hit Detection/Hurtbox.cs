﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hurtbox : MonoBehaviour
{
    public ColliderState colliderState = ColliderState.Open;
    public BoxCollider2D boxCollider;
    //The entity this hurtbox belongs to
    public Entity entity;

    //public Entity 
    public void GetHit(AttackObject attack)
    {
        if(colliderState == ColliderState.Closed)
        {
            return;
        }

        entity.GetHit(attack);
    }

}