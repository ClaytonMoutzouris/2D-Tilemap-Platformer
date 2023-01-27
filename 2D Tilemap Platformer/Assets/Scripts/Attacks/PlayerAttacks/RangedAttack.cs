using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RangedAttack", menuName = "ScriptableObjects/Attacks/PlayerAttacks/WeaponAttacks/RangedAttack")]
public class RangedAttack : WeaponAttack
{

    //A basic attack.
    public override IEnumerator Activate(ButtonInput button = ButtonInput.Fire)
    {
        StartUp();

        //WeaponObject attackObject = player._attackManager.meleeWeapon.SetWeapon(objectPrototypes[0]);

        //Do i really need this class?
        //Can i just know if attack objects are projectiles or not?

        //entity.overrideController["PlayerAttack1"] = ownerAnimation;
        //attacker._animator.Play(attackAnimation.name);
        //attacker._animator.speed = attackSpeed;

        /*
        if (!entity._animator.GetCurrentAnimatorStateInfo(0).IsName(attackAnimation.name))
        {
            yield return null;
        }
        //This checks if the animation has completed one cycle, and won't progress until it has
        //This allows for the animator speed to be adjusted by the "attack speed"
        while (entity._animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1)
        {
            yield return null;
        }
        */

        player._attackManager.rangedWeaponObject.gameObject.SetActive(true);

        while (player._input.GetButton(button))
        {
            player._animator.Play(attackAnimation.name);
            //user._attackManager.rangedWeaponObject.

            weapon.FireAimedProjectile();

            yield return null;
        }


        //float waitTime = attackAnimation.length * (1 / entity._animator.speed);
        //yield return new WaitForSeconds(waitTime);

        //attacker._animator.speed = 1;
        //attacker._animator.Play(Animator.StringToHash("Idle"));
        player._attackManager.rangedWeaponObject.gameObject.SetActive(false);

        CleanUp();
    }
}
