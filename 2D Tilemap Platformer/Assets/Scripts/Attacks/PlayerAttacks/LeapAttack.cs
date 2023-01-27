using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LeapAttack", menuName = "ScriptableObjects/Attacks/PlayerAttacks/WeaponAttacks/LeapAttack")]
public class LeapAttack : WeaponAttack
{
    public float leapHeight = 5;
    public float leapDuration = 0.5f;
    public float leapWeight = 10;
    //public List<AttackObject> activeObjects;

    //A basic attack.
    public override IEnumerator Activate(ButtonInput button = ButtonInput.LightAttack)
    {
        StartUp();

        player._animator.Play(Animator.StringToHash("Jump"));
        float leapTimestamp = Time.time;

        player.movementState = PlayerMovementState.Attacking;
        player._controller.velocity.y = Mathf.Sqrt(2*leapHeight * -GambleConstants.GRAVITY);
        //entity.movementState = PlayerMovementState.Jump;
        

        while(Time.time <= leapTimestamp+leapDuration)
        {
            player._controller.velocity.x = player.GetDirection() * player.stats.GetSecondaryStat(SecondaryStatType.MoveSpeed).GetValue();
            yield return null;
        }

        // Enter the state
        player._controller.velocity.y = -Mathf.Sqrt(2*leapWeight * -GambleConstants.GRAVITY);
        player._controller.velocity.x = 0;
        player._animator.Play(attackAnimation.name);
        player._animator.speed = attackSpeed;

        while (!player._controller.collisionState.below)
        {
            yield return null;
        }

        //Wait for a bit to recover from the impact
        yield return new WaitForSeconds(0.2f);

        if(player.movementState != PlayerMovementState.Dead)
        {
            player.movementState = PlayerMovementState.Idle;
        }
        CleanUp();


    }

}
