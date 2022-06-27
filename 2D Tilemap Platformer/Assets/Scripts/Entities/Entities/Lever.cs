using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour, IHurtable, IInteractable
{
    public bool isTriggered = false;
    public SpriteRenderer spriteRenderer;
    public Animator animator;
    public Hurtbox hurtbox;
    public ITriggerable triggerable;

    public void Awake()
    {
        hurtbox = GetComponentInChildren<Hurtbox>();
        hurtbox.SetOwner(this);
    }

    public void GetHurt(AttackObject attackObject)
    {
        Trigger();
    }

    public void Interact(Entity entity)
    {
        Trigger();
    }

    public void Trigger()
    {
        isTriggered = !isTriggered;

        if(isTriggered)
        {
            animator.Play("Lever_On");
        }
        else
        {
            animator.Play("Lever_Off");

        }

        if(triggerable != null)
        {
            triggerable.Trigger();
        }

    }
}
