using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAttack : Attack
{

    //public List<AttackObject> activeObjects;
    public WeaponObject weaponObject;

    //A basic attack.
    public override IEnumerator Activate(Entity user)
    {
        entity = user;
        StartUp();

        //entity.overrideController["PlayerAttack1"] = ownerAnimation;
        entity._animator.Play(ownerAnimation.name);
        entity._animator.speed = attackSpeed;


        float waitTime = ownerAnimation.length * (1 / entity._animator.speed);

        //attackObject.ActivateObject();
        yield return new WaitForSeconds(waitTime);
        entity._animator.speed = 1;
        entity._animator.Play(Animator.StringToHash("Idle"));
        CleanUp();
    }

}
