using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponChargeAttack : WeaponAttack
{
    public float chargeSpeed = 16;
    public float chargeDuration = 0.5f;

    //A basic attack.
    public override IEnumerator Activate(Entity user)
    {
        entity = user;
        StartUp();

        entity.movementState = MovementState.Charge;
        entity._velocity.x = entity.GetDirection() * chargeSpeed;

        //entity.overrideController["PlayerAttack1"] = ownerAnimation;
        entity._animator.Play(ownerAnimation.name);
        entity._animator.speed = attackSpeed;

        float waitTime = ownerAnimation.length * (1 / entity._animator.speed);

        //attackObject.ActivateObject();

        yield return new WaitForSeconds(waitTime);
        entity._animator.speed = 1;
        entity.movementState = MovementState.Idle;
        entity._animator.Play(Animator.StringToHash("Idle"));
        CleanUp();
    }

}
