using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAttack : Attack
{

    //public List<AttackObject> activeObjects;

    //A basic attack.
    public override IEnumerator Activate(PlayerController user)
    {
        player = user;
        StartUp();

        player._attackManager.meleeWeapon.SetWeapon(player._attackManager.equippedWeapon.weaponObject);
        AttackObject attackObject = player._attackManager.meleeWeapon.weapon.GetComponent<AttackObject>();

        player.overrideController["PlayerAttack1"] = ownerAnimation;
        player._animator.Play(Animator.StringToHash("DEFAULT_ATTACK"));
        player._animator.speed = attackSpeed;


        float waitTime = ownerAnimation.length * (1 / player._animator.speed);

        attackObject.animator.speed = attackSpeed;
        attackObject.ActivateObject();
        attackObject.ActivateObject(waitTime);

        yield return new WaitForSeconds(waitTime);
        player._animator.speed = 1;
        player._animator.Play(Animator.StringToHash("Idle"));
        CleanUp();
    }

}
