using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DashAttack", menuName = "ScriptableObjects/Attacks/DashAttack")]
public class DashAttack : Attack
{
    public float dashSpeed = 16;
    public float dashDuration = 0.5f;

    //A basic attack.
    public override IEnumerator Activate(PlayerController user, ButtonInput button = ButtonInput.LightAttack)
    {
        entity = user;
        StartUp();

        entity.movementState = PlayerMovementState.Attacking;
        dashSpeed = entity.stats.GetSecondaryStat(SecondaryStatType.MoveSpeed).GetValue() * 2;

        //entity.overrideController["PlayerAttack1"] = ownerAnimation;
        entity._animator.Play(attackAnimation.name);
        entity._animator.speed = attackSpeed;

        if (!entity._animator.GetCurrentAnimatorStateInfo(0).IsName(attackAnimation.name))
        {
            yield return null;
        }

        //This checks if the animation has completed one cycle, and won't progress until it has
        //This allows for the animator speed to be adjusted by the "attack speed"
        while (entity._animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1)
        {
            entity._controller.velocity.x = entity.GetDirection() * dashSpeed;
            yield return null;
        }

        //float waitTime = attackAnimation.length * (1 / entity._animator.speed);
        //yield return new WaitForSeconds(waitTime);
        entity._animator.speed = 1;
        if (entity.movementState != PlayerMovementState.Dead)
        {
            entity.movementState = PlayerMovementState.Idle;
        }
        entity._animator.Play(Animator.StringToHash("Idle"));
        CleanUp();
    }

}
