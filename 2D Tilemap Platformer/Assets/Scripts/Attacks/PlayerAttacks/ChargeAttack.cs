using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Rename this to dash attack so i can make a charge up attack aswell
public class ChargeAttack : Attack
{
    public float chargeSpeed = 16;
    public float chargeDuration = 0.5f;

    //A basic attack.
    public override IEnumerator Activate(PlayerController user)
    {
        entity = user;
        StartUp();

        entity.movementState = PlayerMovementState.Attacking;
        entity._velocity.x = entity.GetDirection() * chargeSpeed;

        //entity.overrideController["PlayerAttack1"] = ownerAnimation;
        entity._animator.Play(attackAnimation.name);
        entity._animator.speed = attackSpeed;

        float waitTime = attackAnimation.length * (1 / entity._animator.speed);

        //attackObject.ActivateObject();

        yield return new WaitForSeconds(waitTime);
        entity._animator.speed = 1;
        entity.movementState = PlayerMovementState.Idle;
        entity._animator.Play(Animator.StringToHash("Idle"));
        CleanUp();
    }

}
