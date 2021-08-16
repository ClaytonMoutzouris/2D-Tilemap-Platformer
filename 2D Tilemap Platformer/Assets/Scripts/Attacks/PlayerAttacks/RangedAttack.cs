using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RangedAttack", menuName = "ScriptableObjects/Attacks/RangedAttack")]
public class RangedAttack : Attack
{

    //A basic attack.
    public override IEnumerator Activate(PlayerController user, ButtonInput button = ButtonInput.Fire)
    {
        entity = user;
        StartUp();

        //WeaponObject attackObject = player._attackManager.meleeWeapon.SetWeapon(objectPrototypes[0]);

        //Do i really need this class?
        //Can i just know if attack objects are projectiles or not?

        //entity.overrideController["PlayerAttack1"] = ownerAnimation;
        entity._animator.Play(attackAnimation.name);
        entity._animator.speed = attackSpeed;

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

        float fireRate = user._equipmentManager.equippedWeapon.fireRate;
        float timeStamp = Time.time;

        while(Time.time < timeStamp + fireRate)
        {
            yield return null;
        }
        //float waitTime = attackAnimation.length * (1 / entity._animator.speed);
        //yield return new WaitForSeconds(waitTime);

        entity._animator.speed = 1;
        entity._animator.Play(Animator.StringToHash("Idle"));
        CleanUp();
    }
}
