using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour, IContactable
{
    public bool isTriggered = false;
    public SpriteRenderer spriteRenderer;
    public Animator animator;
    public ITriggerable triggerable;
    public ContactFilter2D contactFilter;
    public BoxCollider2D triggerCollider;

    public void Awake()
    {
        triggerCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
    }

    public void Update()
    {
        List<Collider2D> colliders = new List<Collider2D>();
        BoxCollider2D box = triggerCollider;
        //box.size *= 2;
        Physics2D.OverlapCollider(triggerCollider, contactFilter, colliders);
        //box.size /= 2;

        for (int i = 0; i < colliders.Count; i++)
        {
            Contact();

        }
    }

    public void Contact()
    {
        Trigger();
    }



    public void Trigger()
    {
        isTriggered = true;
        animator.Play("PressurePlate_Triggered");

        if (triggerable != null)
        {
            triggerable.Trigger();
        }

    }
}
