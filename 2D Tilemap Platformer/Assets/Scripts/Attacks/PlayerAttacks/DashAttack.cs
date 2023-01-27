using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DashAttack", menuName = "ScriptableObjects/Attacks/PlayerAttacks/WeaponAttacks/DashAttack")]
public class DashAttack : WeaponAttack
{
    public float dashSpeed = 16;
    public float dashDuration = 0.5f;

    //A basic attack.
    public override IEnumerator Activate(ButtonInput button = ButtonInput.LightAttack)
    {
        StartUp();

        player.movementState = PlayerMovementState.Attacking;
        dashSpeed = player.stats.GetSecondaryStat(SecondaryStatType.MoveSpeed).GetValue() * 2;

        //entity.overrideController["PlayerAttack1"] = ownerAnimation;
        player._animator.Play(attackAnimation.name);
        player._animator.speed = attackSpeed;

        if (!player._animator.GetCurrentAnimatorStateInfo(0).IsName(attackAnimation.name))
        {
            yield return null;
        }

        //This checks if the animation has completed one cycle, and won't progress until it has
        //This allows for the animator speed to be adjusted by the "attack speed"
        while (player._animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1)
        {
            player._controller.velocity.x = player.GetDirection() * dashSpeed;
            yield return null;
        }

        //float waitTime = attackAnimation.length * (1 / entity._animator.speed);
        //yield return new WaitForSeconds(waitTime);
        player._animator.speed = 1;
        if (player.movementState != PlayerMovementState.Dead)
        {
            player.movementState = PlayerMovementState.Idle;
        }
        player._animator.Play(Animator.StringToHash("Idle"));
        CleanUp();
    }

}
