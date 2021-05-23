using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeaponAttack : WeaponAttack
{
    public Projectile projectile;

    public void SetProjectile(Projectile projectile)
    {
        this.projectile = projectile;
    }

    //A basic attack.
    public override IEnumerator Activate(Entity user)
    {
        entity = user;
        StartUp();

        //WeaponObject attackObject = player._attackManager.meleeWeapon.SetWeapon(objectPrototypes[0]);

        Projectile proj = Instantiate(projectile, user.transform.position, Quaternion.identity);
        proj.SetDirection(entity.GetDirection()*Vector2.right);


        //entity.overrideController["PlayerAttack1"] = ownerAnimation;
        entity._animator.Play(ownerAnimation.name);
        entity._animator.speed = attackSpeed;

        float waitTime = ownerAnimation.length * (1 / entity._animator.speed);
        //attackObject.ActivateObject(waitTime);

        //attackObject.animator.speed = attackSpeed;

        yield return new WaitForSeconds(waitTime);
        entity._animator.speed = 1;
        entity._animator.Play(Animator.StringToHash("Idle"));
        CleanUp();
    }
}
