﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class AttackObject : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Animator animator;
    public BoxCollider2D hitbox;
    public ContactFilter2D contactFilter;
    public ColliderState state = ColliderState.Open;

    public List<Collider2D> hits = new List<Collider2D>();
    public float duration = 1;


    public Color inactiveColor;

    public Color collisionOpenColor;

    public Color collidingColor;

    public void ActivateObject()
    {
        animator.Play("Attack");
        StartCoroutine(HandleAttack());
    }

    public void SetDuration(float time)
    {
        duration = time;
    }

    public void ActivateObject(float time)
    {
        SetDuration(time);
        ActivateObject();
    }

    public virtual IEnumerator HandleAttack()
    {

        yield return new WaitForSeconds(duration);
        if(this)
            Destroy(gameObject);
    }


    protected virtual void Update()
    {

        if (state == ColliderState.Closed) { return; }

        //Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position + (Vector3)hitbox.offset, hitbox.size, transform.rotation.z, mask);

        List<Collider2D> colliders = new List<Collider2D>();
        Physics2D.OverlapCollider(hitbox, contactFilter, colliders);

        for (int i = 0; i < colliders.Count; i++)
        {

            Collider2D aCollider = colliders[i];


            CollisionedWith(aCollider);
            
        }

        state = colliders.Count > 0 ? ColliderState.Colliding : ColliderState.Open;

    }

    public virtual void CollisionedWith(Collider2D collider)
    {
        if (hits.Contains(collider))
        {
            return;
        }

        hits.Add(collider);
        Hurtbox hurtbox = collider.GetComponent<Hurtbox>();

        hurtbox?.GetHit(this);
    }

    private void OnDrawGizmos()
    {

        checkGizmoColor();

        Gizmos.matrix = Matrix4x4.TRS(transform.position + (Vector3)hitbox.offset, transform.rotation, transform.localScale);

        Gizmos.DrawWireCube(Vector3.zero, new Vector3(hitbox.size.x*2 + hitbox.offset.x, hitbox.size.y*2 + hitbox.offset.x, 1)); // Because size is halfExtents

    }

    private void checkGizmoColor()
    {
        switch (state)
        {

            case ColliderState.Closed:

                Gizmos.color = inactiveColor;

                break;

            case ColliderState.Open:

                Gizmos.color = collisionOpenColor;

                break;

            case ColliderState.Colliding:

                Gizmos.color = collidingColor;

                break;

        }

    }

}
