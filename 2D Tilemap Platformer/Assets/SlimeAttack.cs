using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeAttack : Attack
{

    public override IEnumerator Activate(Entity user)
    {
        entity = user;
        StartUp();
        //entity.overrideController["Slime_Attack"] = ownerAnimation;
        entity._animator.Play(Animator.StringToHash("Slime_Attack"));
        entity._animator.speed = attackSpeed;

        /*
        foreach (AttackObject attackObj in objectPrototypes)
        {
            AttackObject temp = AttackObject.Instantiate(attackObj, transform);
            temp.animator.speed = attackSpeed;
            float duration = temp.animator.GetCurrentAnimatorStateInfo(0).length * (1 / temp.animator.speed);
            temp.ActivateObject(duration);
        }
        */

        float waitTime = ownerAnimation.length;

        yield return new WaitForSeconds(waitTime);
        entity._animator.speed = 1;
        entity._animator.Play(Animator.StringToHash("Slime_Idle"));
        CleanUp();
    }

}
