using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeAttack : Attack
{

    public float chargeSpeed = 16;
    public float chargeDuration = 0.5f;

    //A basic attack.
    public override IEnumerator Activate(PlayerController player)
    {
        StartUp();

        player.movementState = PlayerMovementState.Charge;
        player._velocity.x = player.GetDirection() * chargeSpeed;
        //yield return new WaitForSeconds(chargeDuration);

        player.overrideController["PlayerAttack1"] = ownerAnimation;
        player._animator.Play(Animator.StringToHash("DEFAULT_ATTACK"));
        player._animator.speed = attackSpeed;

        foreach (AttackObject attackObj in objectPrototypes)
        {
            AttackObject temp = Instantiate(attackObj, transform);
            temp.animator.speed = attackSpeed;
            float duration = temp.animator.GetCurrentAnimatorStateInfo(0).length * (1 / temp.animator.speed);
            temp.ActivateObject(duration);
        }

        Debug.Log("Anim State: " + player._animator.GetCurrentAnimatorStateInfo(0).ToString());
        float waitTime = ownerAnimation.length * (1 / player._animator.speed);
        Debug.Log("Attack waittime: " + waitTime);

        yield return new WaitForSeconds(waitTime);
        player._animator.speed = 1;
        player.movementState = PlayerMovementState.Idle;
        player._animator.Play(Animator.StringToHash("Idle"));
        CleanUp();
    }
}
