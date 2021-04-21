using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeapAttack : Attack
{

    public float leapHeight = 5;
    public float leapDuration = 0.2f;

    //A basic attack.
    public override IEnumerator Activate(PlayerController player)
    {
        StartUp();

        player.movementState = PlayerMovementState.Attacking;
        player._velocity.y = Mathf.Sqrt(2 * leapHeight * -player.gravity);
        player._velocity.x = player.GetDirection()*player.runSpeed;

        yield return new WaitForSeconds(leapDuration);

        player._velocity.y = -Mathf.Sqrt(leapHeight * -player.gravity);
        player._velocity.x = 0;

        player.overrideController["PlayerAttack1"] = ownerAnimation;
        player._animator.Play(Animator.StringToHash("DEFAULT_ATTACK"));
        player._animator.speed = attackSpeed;

        foreach (AttackObject attackObj in objectPrototypes)
        {
            AttackObject temp = AttackObject.Instantiate(attackObj, transform);
            temp.animator.speed = attackSpeed;
            float duration = temp.animator.GetCurrentAnimatorStateInfo(0).length * (1 / temp.animator.speed);
            temp.ActivateObject(duration);
        }

        float waitTime = ownerAnimation.length * (1 / player._animator.speed);

        yield return new WaitForSeconds(waitTime);
        player._animator.speed = 1;
        player.movementState = PlayerMovementState.Idle;
        player._animator.Play(Animator.StringToHash("Idle"));
        CleanUp();
    }
}
