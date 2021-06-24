using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DashAttack", menuName = "ScriptableObjects/Attacks/DashAttack")]
public class DashAttack : Attack
{
    public float dashSpeed = 16;
    public float dashDuration = 0.5f;

    //A basic attack.
    public override IEnumerator Activate(PlayerController user)
    {
        entity = user;
        StartUp();

        entity.movementState = PlayerMovementState.Attacking;
        dashSpeed = entity.movementSpeed * 2;
        entity._velocity.x = entity.GetDirection() * dashSpeed;

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
