using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : Entity, IHurtable, IInteractable
{
    public bool isTriggered = false;
    public Animator animator;
    public Hurtbox hurtbox;
    public ITriggerable triggerable;

    protected override void Awake()
    {
        base.Awake();
        hurtbox = GetComponentInChildren<Hurtbox>();
        hurtbox.SetOwner(this);
    }

    public bool CheckFriendly(Entity entity)
    {
        return false;
    }

    public void GetHurt(ref AttackHitData hit)
    {
        Trigger();
    }

    public bool CheckHit(AttackObject attack)
    {
        return true;
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

    public Health GetHealth()
    {
        return null;
    }

    public Hurtbox GetHurtbox()
    {
        return hurtbox;
    }

}
