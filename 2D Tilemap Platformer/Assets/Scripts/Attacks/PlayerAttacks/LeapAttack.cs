using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LEAP_ATTACK_STATE {
    LEAP = 0,
    FALL = 1,
    IMPACT = 2
}

public class LeapAttack : Attack
{
    public LEAP_ATTACK_STATE state = 0;
    public float leapHeight = 5;
    public float leapDuration = 0.3f;
    //public List<AttackObject> activeObjects;

    //A basic attack.
    public override IEnumerator Activate(PlayerController user)
    {
        entity = user;
        StartUp();
        state = LEAP_ATTACK_STATE.LEAP;

        while (true)
        {
            yield return StartCoroutine(state.ToString());
        }

    }

    IEnumerator LEAP()
    {
        while (state == LEAP_ATTACK_STATE.LEAP)
        {
            entity._animator.Play(Animator.StringToHash("Jump"));

            entity.movementState = PlayerMovementState.Attacking;
            entity._velocity.y = Mathf.Sqrt(2 * leapHeight * -GambleConstants.GRAVITY);
            entity._velocity.x = entity.GetDirection() * entity.movementSpeed;
            yield return new WaitForSeconds(leapDuration);
            state = LEAP_ATTACK_STATE.FALL;

            yield return null;
        }

    }

    IEnumerator FALL()
    {
        // Enter the state
        entity._velocity.y = -Mathf.Sqrt(leapHeight * -GambleConstants.GRAVITY);
        entity._velocity.x = 0;
        entity._animator.Play(attackAnimation.name);
        entity._animator.speed = attackSpeed;

        while (state == LEAP_ATTACK_STATE.FALL)
        {
            //Fall until we hit the ground, consider future exit conditions
            if(entity._controller.collisionState.below)
            {
                state = LEAP_ATTACK_STATE.IMPACT;
            }

            yield return null;
        }

    }

    IEnumerator IMPACT()
    {

        while (state == LEAP_ATTACK_STATE.IMPACT)
        {
            //Wait for a bit to recover from the impact
            yield return new WaitForSeconds(0.2f);

            entity.movementState = PlayerMovementState.Idle;
            CleanUp();

            yield return null;
        }

    }

}
