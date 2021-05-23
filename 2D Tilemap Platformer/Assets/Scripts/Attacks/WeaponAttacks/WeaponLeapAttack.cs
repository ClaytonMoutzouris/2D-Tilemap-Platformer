using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponLeapAttack : WeaponAttack
{
    public float leapHeight = 5;
    public float leapDuration = 0.2f;
    //public List<AttackObject> activeObjects;

    //A basic attack.
    public override IEnumerator Activate(Entity user)
    {
        entity = user;
        StartUp();

        entity.movementState = MovementState.Attacking;
        entity._velocity.y = Mathf.Sqrt(2 * leapHeight * -GambleConstants.GRAVITY);
        entity._velocity.x = entity.GetDirection() * entity.movementSpeed;

        yield return new WaitForSeconds(leapDuration);

        entity._velocity.y = -Mathf.Sqrt(leapHeight * -GambleConstants.GRAVITY);
        entity._velocity.x = 0;

        //entity.overrideController["PlayerAttack1"] = ownerAnimation;
        entity._animator.Play(ownerAnimation.name);
        entity._animator.speed = attackSpeed;


        float waitTime = ownerAnimation.length * (1 / entity._animator.speed);

        //attackObject.ActivateObject();

        yield return new WaitForSeconds(waitTime);
        entity._animator.speed = 1;
        entity.movementState = MovementState.Idle;
        entity._animator.Play(Animator.StringToHash("Idle"));
        CleanUp();
    }

}
