using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponLeapAttack : WeaponAttack
{
    public float leapHeight = 5;
    public float leapDuration = 0.2f;
    //public List<AttackObject> activeObjects;

    //A basic attack.
    public override IEnumerator Activate(PlayerController user)
    {
        player = user;
        StartUp();

        player.movementState = PlayerMovementState.Attacking;
        player._velocity.y = Mathf.Sqrt(2 * leapHeight * -player.gravity);
        player._velocity.x = player.GetDirection() * player.runSpeed;

        yield return new WaitForSeconds(leapDuration);

        player._velocity.y = -Mathf.Sqrt(leapHeight * -player.gravity);
        player._velocity.x = 0;

        player._attackManager.meleeWeapon.SetWeapon(player._attackManager.equippedWeapon.weaponObject);
        AttackObject attackObject = player._attackManager.meleeWeapon.weapon.GetComponent<AttackObject>();

        player.overrideController["PlayerAttack1"] = ownerAnimation;
        player._animator.Play(Animator.StringToHash("DEFAULT_ATTACK"));
        player._animator.speed = attackSpeed;


        float waitTime = ownerAnimation.length * (1 / player._animator.speed);

        attackObject.animator.speed = attackSpeed;
        //attackObject.ActivateObject();
        attackObject.ActivateObject(waitTime);

        yield return new WaitForSeconds(waitTime);
        player._animator.speed = 1;
        player.movementState = PlayerMovementState.Idle;
        player._animator.Play(Animator.StringToHash("Idle"));
        CleanUp();
    }

}
