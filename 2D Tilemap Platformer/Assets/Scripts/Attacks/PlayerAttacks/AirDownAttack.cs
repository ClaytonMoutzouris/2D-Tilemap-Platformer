using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AirDownAttack", menuName = "ScriptableObjects/Attacks/PlayerAttacks/WeaponAttacks/AirDownAttack")]
public class AirDownAttack : WeaponAttack
{
    public float fallSpeedMod = 5;

    //A basic attack.
    public override IEnumerator Activate(ButtonInput button = ButtonInput.LightAttack)
    {
        StartUp();

        player.movementState = PlayerMovementState.Attacking;
        //entity.movementState = PlayerMovementState.Jump;

        // Enter the state
        player._controller.velocity.y = -Mathf.Sqrt(2 * fallSpeedMod * -GambleConstants.GRAVITY);
        player._controller.velocity.x = player.GetDirection() * player.stats.GetSecondaryStat(SecondaryStatType.MoveSpeed).GetValue()*.075f;
        player._animator.Play(attackAnimation.name);
        player._animator.speed = attackSpeed;

        while (!player._controller.collisionState.below)
        {
            yield return null;
        }

        //Wait for a bit to recover from the impact
        yield return new WaitForSeconds(0.2f);

        if (player.movementState != PlayerMovementState.Dead)
        {
            player.movementState = PlayerMovementState.Idle;
        }

        CleanUp();


    }

}
