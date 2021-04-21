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
    public override IEnumerator Activate(PlayerController user)
    {
        player = user;
        StartUp();

        //WeaponObject attackObject = player._attackManager.meleeWeapon.SetWeapon(objectPrototypes[0]);
        player._attackManager.meleeWeapon.SetWeapon(player._attackManager.equippedWeapon.weaponObject);

        Projectile proj = Instantiate(projectile, user.transform.position, Quaternion.identity);
        proj.SetDirection(player.GetDirection()*Vector2.right);


        player.overrideController["PlayerAttack1"] = ownerAnimation;
        player._animator.Play(Animator.StringToHash("DEFAULT_ATTACK"));
        player._animator.speed = attackSpeed;

        float waitTime = ownerAnimation.length * (1 / player._animator.speed);
        //attackObject.ActivateObject(waitTime);

        //attackObject.animator.speed = attackSpeed;
        user._attackManager.StartCoroutine(proj.HandleAttack());

        yield return new WaitForSeconds(waitTime);
        player._animator.speed = 1;
        player._animator.Play(Animator.StringToHash("Idle"));
        Destroy(player._attackManager.meleeWeapon.weapon.gameObject);
        CleanUp();
    }
}
