using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LeapAttack", menuName = "ScriptableObjects/Attacks/LeapAttack")]
public class LeapAttack : Attack
{
    public float leapHeight = 5;
    public float leapDuration = 0.5f;
    public float leapWeight = 10;
    //public List<AttackObject> activeObjects;

    //A basic attack.
    public override IEnumerator Activate(PlayerController user, ButtonInput button = ButtonInput.LightAttack)
    {
        entity = user;
        StartUp();

        entity._animator.Play(Animator.StringToHash("Jump"));
        float leapTimestamp = Time.time;

        entity.movementState = PlayerMovementState.Attacking;
        entity._velocity.y = Mathf.Sqrt(2*leapHeight * -GambleConstants.GRAVITY);
        //entity.movementState = PlayerMovementState.Jump;
        

        while(Time.time <= leapTimestamp+leapDuration)
        {
            entity._velocity.x = entity.GetDirection() * entity.stats.GetSecondaryStat(SecondaryStatType.MoveSpeed).GetValue();
            yield return null;
        }

        // Enter the state
        entity._velocity.y = -Mathf.Sqrt(2*leapWeight * -GambleConstants.GRAVITY);
        entity._velocity.x = 0;
        entity._animator.Play(attackAnimation.name);
        entity._animator.speed = attackSpeed;

        while (!entity._controller.collisionState.below)
        {
            yield return null;
        }

        //Wait for a bit to recover from the impact
        yield return new WaitForSeconds(0.2f);

        if(entity.movementState != PlayerMovementState.Dead)
        {
            entity.movementState = PlayerMovementState.Idle;
        }
        CleanUp();


    }

}
