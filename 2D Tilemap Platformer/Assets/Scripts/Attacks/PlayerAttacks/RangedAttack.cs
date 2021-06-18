using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttack : Attack
{
    public Projectile projectile;

    public void SetProjectile(Projectile projectile)
    {
        this.projectile = projectile;
    }

    //A basic attack.
    public override IEnumerator Activate(PlayerController user)
    {
        entity = user;
        StartUp();

        //WeaponObject attackObject = player._attackManager.meleeWeapon.SetWeapon(objectPrototypes[0]);




        //entity.overrideController["PlayerAttack1"] = ownerAnimation;
        entity._animator.Play(attackAnimation.name);
        entity._animator.speed = attackSpeed;

        float waitTime = attackAnimation.length * (1 / entity._animator.speed);
        //attackObject.ActivateObject(waitTime);

        //attackObject.animator.speed = attackSpeed;

        yield return new WaitForSeconds(waitTime);
        entity._animator.speed = 1;
        entity._animator.Play(Animator.StringToHash("Idle"));
        CleanUp();
    }
}
